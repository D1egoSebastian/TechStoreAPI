//using Microsoft.AspNetCore.Components;
using TechStoreAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetCategories()
    {
        var Categorylist = _context.Categories.ToList();

        if (!Categorylist.Any())
        {
            return NotFound("Not categories to show");
        }

        return Ok(Categorylist);
    }
}