using System.ComponentModel.DataAnnotations;

public class CreateProductDto
{
    [Required]
    public string? Name {get; set;}
    public string? Description {get; set;}
    public int Price {get; set;}
    public int Stock {get; set;}
    public int CategoryId {get;set;}
    public string? Image_url {get;set;}
    public DateTime Created_at {get;set;}
}