namespace TechStoreAPI.Models
{
    public class Cart
    {
        public int id {  get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public List<CartItem> CartItems { get; set; }
    }

}
