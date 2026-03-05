using Microsoft.AspNetCore.Mvc;
using TechStoreAPI.Data;

[Route("api/[controller]")]
[ApiController]

public class CartController : ControllerBase
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }
}