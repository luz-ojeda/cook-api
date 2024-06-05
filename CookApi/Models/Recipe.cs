namespace CookApi.Models;

public class Recipe
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Summary { get; set; }
    public required List<string> Ingredients { get; set; }
    public required string Instructions { get; set; }
    public List<string>? Pictures { get; set; } = [];
    public List<string>? Videos { get; set; } = [];
    public int? PreparationTime { get; set; }
    public int? CookingTime { get; set; }
    public int? Servings { get; set; }
    public RecipeDifficulty? Difficulty { get; set; }
    public bool? Vegetarian { get; set; }
    public bool UserCreated { get; set; }
}
