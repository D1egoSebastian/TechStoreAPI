using System.ComponentModel.DataAnnotations;
public class CreateCategoryDto
{
    [Required]
    public string Name { get; set;}
    public string? Description { get; set; }
}