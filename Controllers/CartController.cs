using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreAPI.Data;
using TechStoreAPI.Models;

[Route("api/[controller]")]
[ApiController]

public class CartController : ControllerBase
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task <IActionResult> GetCart()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var findcart = await _context.Carts
        .Include(i => i.CartItems)
        .ThenInclude(p => p.Product)
        .FirstOrDefaultAsync(x => x.UserId == userId);

        if(findcart == null)
        {
            return BadRequest("this user doesnt have a cart!");
        }

        return Ok(new 
        {
            CarId = findcart.id,
            Caritems = findcart.CartItems?.Select(ci => new
            {
                Productid = ci.ProductId,
                ProductName = ci.Product?.Name,
                ProductPrice = ci.Product?.Price,
                Quantity = ci.Quantity
            })
        });
    }

    [HttpPost]
    [Authorize]
    public async Task <IActionResult> AddCart()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var findcart = await _context.Carts
        .Include(i => i.CartItems)
        .FirstOrDefaultAsync(x => x.UserId == userId);

        if(findcart == null)
        {
            var newCartToUser = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(newCartToUser);
            await _context.SaveChangesAsync();
        }

        //Check if the product exist
        //Check if the quiantity is valid
        //Check if the product is already on the cart, it is, only add the quantity

        
    }
}