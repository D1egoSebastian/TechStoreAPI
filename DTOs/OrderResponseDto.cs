namespace TechStoreAPI.DTOs
{
    public class OrderResponseDto
{
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponseDto>? Items {get;set;}
}

public class OrderItemResponseDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}