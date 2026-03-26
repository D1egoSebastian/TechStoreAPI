using System.ComponentModel.DataAnnotations;

namespace TechStoreAPI.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Description { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }
        
        [Required]
        public int Stock { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        public string? Image_url { get; set; }
        
        public IFormFile? Image { get; set; }
    }
}

