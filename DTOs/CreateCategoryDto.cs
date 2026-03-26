using System.ComponentModel.DataAnnotations;

namespace TechStoreAPI.DTOs
{
    public class CreateCategoryDto
{
    [Required]
    public string? Name { get; set;}
    public string? Description { get; set; }
}
}
