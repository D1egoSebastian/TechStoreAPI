
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

    public ProductController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
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

        string? imageUrl = null;

        if (dto.ImageFile != null && dto.ImageFile.Length > 0)
        {
            using var stream = dto.ImageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(dto.ImageFile.FileName, stream),
                Folder = "techstore"
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.Error != null)
            {
                return BadRequest(new { message = result.Error.Message });
            }
            imageUrl = result.SecureUrl.ToString();
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

        var categoryexist = await _context.Categories.AnyAsync(x => x.Id == dto.CategoryId);

        if(!categoryexist)
        {
            return BadRequest(new { message = "Category does not exist" });
        }

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
            CategoryId = producttofind.CategoryId,
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

        if (dto.ImageFile != null && dto.ImageFile.Length > 0)
        {
            using var stream = dto.ImageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(dto.ImageFile.FileName, stream),
                Folder = "techstore"
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.Error != null)
            {
                return BadRequest(new { message = result.Error.Message });
            }
            productexist.ImageUrl = result.SecureUrl.ToString();
        }
        else if (!string.IsNullOrEmpty(dto.Image_url))
        {
            productexist.ImageUrl = dto.Image_url;
        }
        
        productexist.Name = dto.Name;
        productexist.Description = dto.Description;
        productexist.Price = dto.Price;
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

        _context.Products.Remove(producttofind);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("seed")]
    [Consumes("application/json")]
    public async Task<IActionResult> SeedData()
    {
        if (await _context.Products.AnyAsync())
        {
            _context.Products.RemoveRange(_context.Products);
            await _context.SaveChangesAsync();
        }
        
        if (await _context.Categories.AnyAsync())
        {
            _context.Categories.RemoveRange(_context.Categories);
            await _context.SaveChangesAsync();
        }

        var categories = new List<Category>
        {
            new Category { Name = "Smartphones", Description = "Latest smartphones and mobile devices" },
            new Category { Name = "Laptops", Description = "Computers and laptops" },
            new Category { Name = "Audio", Description = "Headphones, speakers and audio equipment" },
            new Category { Name = "Gaming", Description = "Consoles, games and accessories" },
            new Category { Name = "Accessories", Description = "Cables, chargers and other accessories" }
        };

        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();

        var products = new List<Product>
        {
            new Product { Name = "iPhone 15 Pro", Description = "Latest Apple smartphone with titanium design", Price = 999.99, Stock = 50, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "Samsung Galaxy S24", Description = "Premium Android smartphone with AI features", Price = 899.99, Stock = 45, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "MacBook Pro 16", Description = "Powerful laptop with M3 chip", Price = 2499.99, Stock = 20, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "Dell XPS 15", Description = "Premium Windows laptop", Price = 1799.99, Stock = 25, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1593642632559-0c6d3fc62b89?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "AirPods Pro", Description = "Active noise cancellation earbuds", Price = 249.99, Stock = 100, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1600294037681-c80b4cb5b434?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "Sony WH-1000XM5", Description = "Premium wireless headphones", Price = 399.99, Stock = 60, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "PlayStation 5", Description = "Next-gen gaming console", Price = 499.99, Stock = 15, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "Xbox Series X", Description = "Powerful gaming console", Price = 499.99, Stock = 15, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1621259182978-fbf93132d53d?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "USB-C Hub", Description = "7-in-1 USB-C adapter", Price = 49.99, Stock = 200, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1625723044792-44de16ccb4e9?w=400", CreatedAt = DateTime.UtcNow },
            new Product { Name = "Wireless Charger", Description = "Fast wireless charging pad", Price = 29.99, Stock = 150, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1586816879360-004f5b0c51e3?w=400", CreatedAt = DateTime.UtcNow }
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Seed data created successfully", categories = categories.Count, products = products.Count });
    }
}
