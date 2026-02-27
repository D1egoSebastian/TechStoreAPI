//using Microsoft.AspNetCore.Components;
using TechStoreAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TechStoreAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var Categorylist = await _context.Categories.ToListAsync();

        if (!Categorylist.Any())
        {
            return NotFound( new {message = "Not categories to show"});
        }

        return Ok(Categorylist);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newCategory = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        var CategoryExist =  await _context.Categories.AnyAsync(x => x.Name == newCategory.Name);

        if(CategoryExist)
        {
            return BadRequest("Already exist!");
        }

        _context.Categories.Add(newCategory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategories), new
        {
            id = newCategory.Id
        }, new CategoryResponseDto
        {
            Id = newCategory.Id,
            Name = newCategory.Name,
            Description = newCategory.Description
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var categorytofind = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (categorytofind == null)
        {
            return NotFound(new{message= "category not found"});
        }

         return Ok(new CategoryResponseDto
         {
             Id = categorytofind.Id,
             Name = categorytofind.Name,
             Description = categorytofind.Description
         });
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> UpdateCategory([FromBody] CreateCategoryDto dto, int id)
    {

         if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var categorytofind = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (categorytofind == null)
        {
            return NotFound(new{message= "category not found"});
        }

        categorytofind.Name = dto.Name;
        categorytofind.Description = dto.Description;

        await _context.SaveChangesAsync();

        return Ok(new CategoryResponseDto
         {
             Id = categorytofind.Id,
             Name = categorytofind.Name,
             Description = categorytofind.Description
         });


    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteCategory(int id)
    {
        var categorytofind = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (categorytofind == null)
        {
            return NotFound(new{message= "category not found"});
        }

        _context.Categories.Remove(categorytofind);
      await _context.SaveChangesAsync();

        return NoContent();
    }

}