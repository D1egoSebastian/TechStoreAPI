using TechStoreAPI.Models;

namespace TechStoreAPI.DTOs
{
    public class ProductResponseDto
{
    public int Id {get;set;}
     public string? Name {get; set;}
    public string? Description {get; set;}
    public double Price {get; set;}
    public int Stock {get; set;}
    public int CategoryId {get;set;}
    public string? Image_url {get;set;}

    public CategoryResponseDto? Category{get;set;}
}
}
