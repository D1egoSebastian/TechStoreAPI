
using System.ComponentModel.DataAnnotations;

namespace TechStoreAPI.DTOs
{
    public class RegisterDto {
        [Required]
        public string? Name {get;set;}
        [Required]
        [EmailAddress]
        public string? Email {get;set;}
        [Required]
        public string? PasswordHash{get;set;}
    }
    
}