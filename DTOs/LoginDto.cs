
using System.ComponentModel.DataAnnotations;

namespace TechStoreAPI.DTOs
{
    public class LoginDto {
        [Required]
        public string? Email {get;set;}
        [Required]
        public string? PasswordHash{get;set;}
    }
    
}