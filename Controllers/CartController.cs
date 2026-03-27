using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreAPI.Data;
using TechStoreAPI.DTOs;
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
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name,
                ProductPrice = ci.Product?.Price,
                Quantity = ci.Quantity
            })
        });
    }

    [HttpPost]
    [Authorize]
    public async Task <IActionResult> AddItemToCart([FromBody] CreateCartDto dto)
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
            findcart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(findcart);
            await _context.SaveChangesAsync();
        }

        //Check if the product exist
        var productexist = await _context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);

        if(productexist == null)
        {
            return NotFound(new { message = "product not found"});
        }

        //Check if the quantity is valid
        if(productexist.Stock < dto.Quantity)
        {
            return BadRequest(new { message = "no stock."});
        }

        //Check if the product is already on the cart, it is, only add the quantity
        var productInCart = findcart.CartItems?.FirstOrDefault(x => x.ProductId == dto.ProductId);

        if (productInCart == null)
        {
            var newItem = new CartItem
            {
                CartId = findcart.id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            _context.CartItems.Add(newItem);
            
        } else
        {
            productInCart.Quantity = dto.Quantity;
        }

        
        await _context.SaveChangesAsync();

        return Ok(new {message = "item added to cart!"});
        
    }

    [HttpPut("{productId}")]
    [Authorize]
    public async Task<IActionResult> UpdateCart(int productId, [FromBody] UpdateCartDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var cartitemFinder = await _context.CartItems
        .Include(c => c.Cart)
        .FirstOrDefaultAsync(c => c.ProductId == productId && c.Cart.UserId == userId );

        if(cartitemFinder == null)
        {
            return NotFound(new {message = "item not found in cart!"});
        }

        if(dto.Quantity == 0)
        {
            _context.CartItems.Remove(cartitemFinder);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        cartitemFinder.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();

        return Ok (new
        {
            ProductId = cartitemFinder.ProductId,
            Quantity = cartitemFinder.Quantity
        });
    }

    [HttpDelete("{productId}")]
    [Authorize]
    public async Task<IActionResult> DeleteItemCart(int productId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var cartitemFinder = await _context.CartItems
        .Include(c => c.Cart)
        .FirstOrDefaultAsync(c => c.ProductId == productId && c.Cart.UserId == userId );

        if(cartitemFinder == null)
        {
            return NotFound(new {message = "item not found in cart!"});
        }

        _context.CartItems.Remove(cartitemFinder);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}