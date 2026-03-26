
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreAPI.Data;
using TechStoreAPI.Models;
using TechStoreAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

[Route("api/[controller]")]
[ApiController]

public class ProductController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly Cloudinary _cloudinary;

    public ProductController(AppDbContext context, Cloudinary cloudinary)
    {
        _context = context;
        _cloudinary = cloudinary;
    }

    private async Task<string?> UploadImageToCloudinary(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return null;

        var extension = Path.GetExtension(file.FileName).ToLower();
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        
        if (!validExtensions.Contains(extension))
            return null;

        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "techstore/products"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }

    private async Task DeleteImageFromCloudinary(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return;

        try
        {
            var publicId = GetPublicIdFromUrl(imageUrl);
            if (!string.IsNullOrEmpty(publicId))
            {
                await _cloudinary.DestroyAsync(new DeletionParams(publicId));
            }
        }
        catch { }
    }

    private string GetPublicIdFromUrl(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var path = uri.AbsolutePath;
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        
        if (segments.Length >= 2 && segments[0] == "upload")
        {
            var publicIdParts = segments.Skip(2).ToList();
            var lastIndex = publicIdParts.LastIndexOf(Path.GetFileNameWithoutExtension(path));
            if (lastIndex > 0)
                publicIdParts = publicIdParts.Take(lastIndex).ToList();
            return string.Join("/", publicIdParts);
        }
        
        return path.Replace("/upload/", "").Replace(Path.GetExtension(path), "");
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var product = await _context.Products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Image_url = p.ImageUrl,
            Category = new CategoryResponseDto
            {
                Id = p.Category.Id,
                Name = p.Category.Name,
                Description = p.Category.Description
            }
        }).ToListAsync();

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddProduct([FromForm] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var categoryexist = await _context.Categories.AnyAsync(x => x.Id == dto.CategoryId);

        if(!categoryexist)
        {
            return BadRequest(new { message = "Category does not exist" });
        }

        string? imageUrl = null;
        if (dto.Image != null)
        {
            imageUrl = await UploadImageToCloudinary(dto.Image);
            if (imageUrl == null)
            {
                return BadRequest(new { message = "Invalid image format. Allowed: jpg, jpeg, png, gif, webp" });
            }
        }

        var newProduct = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            ImageUrl = imageUrl ?? dto.Image_url,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(newProduct);
        await  _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProductById), new
        {
            id = newProduct.Id
        }, new ProductResponseDto
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            Description = newProduct.Description,
            Price = newProduct.Price,
            CategoryId = newProduct.CategoryId,
            Image_url = newProduct.ImageUrl,
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var producttofind = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(x => x.Id == id);

        if (producttofind == null)
        {
            return NotFound(new {message = "that product not exist!"});
        }

        return Ok(new ProductResponseDto
        {
            Id = producttofind.Id,
            Name = producttofind.Name,
            Description = producttofind.Description,
            Price = producttofind.Price,
            Stock = producttofind.Stock,
            Image_url = producttofind.ImageUrl,
            Category = new CategoryResponseDto
            {
                Id = producttofind.Category.Id,
                Name = producttofind.Category.Name,
                Description = producttofind.Category.Description
            }

        });
        
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var productexist = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(x => x.Id == id);

        if (productexist == null)
        {
            return NotFound(new {message = "that product not exist!"});
        }

        var categoryexist = await _context.Categories.AnyAsync(x => x.Id == dto.CategoryId);

        if(!categoryexist)
        {
            return BadRequest();
        }
        
        string? newImageUrl = null;
        
        if (dto.Image != null)
        {
            newImageUrl = await UploadImageToCloudinary(dto.Image);
            if (newImageUrl == null)
            {
                return BadRequest(new { message = "Invalid image format. Allowed: jpg, jpeg, png, gif, webp" });
            }
            
            if (!string.IsNullOrEmpty(productexist.ImageUrl))
            {
                await DeleteImageFromCloudinary(productexist.ImageUrl);
            }
            
            productexist.ImageUrl = newImageUrl;
        }
        else if (!string.IsNullOrEmpty(dto.Image_url))
        {
            productexist.ImageUrl = dto.Image_url;
        }

        productexist.Name = dto.Name;
        productexist.Description = dto.Description;
        productexist.Price = dto.Price;
        productexist.Stock = dto.Stock;
        productexist.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return Ok(new ProductResponseDto
        {
            Id = productexist.Id,
            Name = productexist.Name,
            Description = productexist.Description,
            Price = productexist.Price,
            Image_url = productexist.ImageUrl,
            Category = new CategoryResponseDto
            {
                Id = productexist.Category.Id,
                Name = productexist.Category.Name,
                Description = productexist.Category.Description
            }
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var producttofind = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (producttofind == null)
        {
            return NotFound(new {message = "that product not exist!"});
        }

        if (!string.IsNullOrEmpty(producttofind.ImageUrl))
        {
            await DeleteImageFromCloudinary(producttofind.ImageUrl);
        }

        _context.Products.Remove(producttofind);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
