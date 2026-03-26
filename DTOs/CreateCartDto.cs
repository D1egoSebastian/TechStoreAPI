using System.ComponentModel.DataAnnotations;

namespace TechStoreAPI.DTOs
{
    public class CreateCartDto
    {
        [Required]
        public int ProductId {get;set;}
        [Required]
        public int Quantity {get;set;}
    }
}