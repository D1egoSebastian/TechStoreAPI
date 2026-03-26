

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreAPI.Data;
using TechStoreAPI.DTOs;
using TechStoreAPI.Models;

[Route("api/[controller]")]
[ApiController]

public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddOrder()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var CartInUser = await _context.CartItems
        .Include(p => p.Product)
        .Where(p => p.Cart.UserId == userId).ToListAsync();

        if(CartInUser == null)
        {
            return NotFound(new {message = "this user have no products or cart"});
        }

        
        var newOrder = new Order
        {

            // cannot convert double to decimal;
            TotalAmount = CartInUser.Sum(item => (decimal)item.Product.Price * item.Quantity),
            Status = "Pending",
            UserId = userId,
        };

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        foreach (var item in CartInUser)
        {
            var newOrderItem = new OrderItem
            {
                OrderId = newOrder.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = (int)item.Product.Price
            };

            _context.OrderItems.Add(newOrderItem);
            item.Product.Stock -= item.Quantity;
        }

        _context.CartItems.RemoveRange(CartInUser);
        await _context.SaveChangesAsync();

        return Ok( new OrderResponseDto
        {
            Id = newOrder.Id,
            TotalAmount = newOrder.TotalAmount,
            Status = newOrder.Status,
            CreatedAt = DateTime.UtcNow,
            Items = CartInUser.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                Quantity = item.Quantity,
                UnitPrice = (int)item.Product.Price
            }).ToList()
        });
        
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetOrders()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var orders = await _context.Orders
        .Include( o => o.OrderItems)
        .ThenInclude(p => p.Product)
        .Where(x => x.UserId == userId)
        .ToListAsync();

        return Ok(orders.Select(o => new OrderResponseDto
        {
            Id = o.Id,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            Items = o.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                Quantity = item.Quantity,
                UnitPrice = (int)item.Product.Price
            }).ToList()
        }));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task <IActionResult> GetOrderById(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var orderById = await _context.Orders
                        .Include(o => o.OrderItems)
                        .ThenInclude(p => p.Product)
                        .FirstOrDefaultAsync(u => u.UserId == userId && u.Id == id);

        if (orderById == null)
        {
            return NotFound(new {message = "that order by id dont exist or the user with that order dont exist."});
        }

        return Ok( new OrderResponseDto
        {
            Id = orderById.Id,
            TotalAmount = orderById.TotalAmount,
            Status = orderById.Status,
            CreatedAt = orderById.CreatedAt,
            Items = orderById.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                Quantity = item.Quantity,
                UnitPrice = (int)item.Product.Price
            }).ToList()
        });
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task <IActionResult> CancelOrder(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if(userId == null)
        {
            return Unauthorized();
        }

        var orderById = await _context.Orders
        .Where(x => x.UserId == userId)
        .FirstOrDefaultAsync( u => u.Id == id);

        if(orderById == null)
        {
            return NotFound(new { message = "order not found" });
        }
        if(orderById.Status != "Pending")
        {
            return BadRequest(new { message = "action denied."});
        }

        orderById.Status = "Cancelled";
        await _context.SaveChangesAsync();

        return Ok(new OrderResponseDto
        {
            Id = orderById.Id,
            TotalAmount = orderById.TotalAmount,
            Status = orderById.Status,
            CreatedAt = orderById.CreatedAt
        });

    }
}