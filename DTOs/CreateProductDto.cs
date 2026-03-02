using System.ComponentModel.DataAnnotations;

namespace TechStoreAPI.DTOs
{
    public class CreateProductDto
{
    [Required]
    public string? Name {get; set;}
    public string? Description {get; set;}
    public double Price {get; set;}
    public int Stock {get; set;}
    public int CategoryId {get;set;}
    public string? Image_url {get;set;}
}
}

