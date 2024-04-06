using System.ComponentModel.DataAnnotations;

namespace CookApi.DTOs;

public class RecipeDTO
{
    public Guid Id { get; set; }

    [Required, MinLength(1), MaxLength(150)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚâêîôÂÊÎÔãõÃÕàèìòùÀÈÌÒÙäëïöüÄËÏÖÜÑñçÇ,''\-!&¡\s]{1,40}$", 
        ErrorMessage = "Used invalid characters in the recipe name.")]
    [Display(Name = "Recipe name")]
    public required string Name { get; set; }

    [MinLength(10), MaxLength(150)]
    public string? Summary { get; set; }

    [Required, MinLength(1, ErrorMessage = "At least one ingredient is required")]
    public required List<string> Ingredients { get; set; }

    [Required, MinLength(20)]
    public required string Instructions { get; set; }

    public List<string> Pictures { get; set; } = [];

    public List<string> Videos { get; set; } = [];

    [Range(1, 300)]
    [Display(Name = "Preparation time")]
    public int? PreparationTime { get; set; }

    [Range(1, 300)]
    [Display(Name = "Cooking time")]
    public int? CookingTime { get; set; }

    [Range(1, 50)]
    public int? Servings { get; set; }

    public string? Difficulty { get; set; }

    public bool Vegetarian { get; set; }
}
