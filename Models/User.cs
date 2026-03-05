namespace TechStoreAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role {get;set;} = "Customer";

        public Cart? Cart { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
