

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreAPI.Data;
using TechStoreAPI.Models;

[Route("api/[controller]")]
[ApiController]

public class ProductController : ControllerBase {
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
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
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var newProduct = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            ImageUrl = dto.Image_url,
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
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDto dto)
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
        
        productexist.Name = dto.Name;
        productexist.Description = dto.Description;
        productexist.Price = dto.Price;
        productexist.CategoryId = dto.CategoryId;
        productexist.ImageUrl = dto.Image_url;


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
}
