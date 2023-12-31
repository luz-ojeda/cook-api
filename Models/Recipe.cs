using System.ComponentModel.DataAnnotations;

namespace CookApi.Models;

public enum RecipeDifficulty
{
    Easy,
    Medium,
    Hard
}

public class Recipe
{
    public Guid Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; }

    [MaxLength(50)]
    public List<string> Ingredients { get; set; }
    public string Instructions { get; set; }

    public List<string> Pictures { get; set; } = new List<string>(); // Default to an empty list

    [StringLength(200)]
    public List<string> Videos { get; set; } = new List<string>();   // Default to an empty list
    public int PreparationTime { get; set; }
    public int CookingTime { get; set; }
    public int Servings { get; set; }
    public RecipeDifficulty Difficulty { get; set; }
    public bool Vegetarian { get; set; } = false;
}
