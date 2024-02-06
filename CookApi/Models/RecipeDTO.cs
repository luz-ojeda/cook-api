public class RecipeDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required List<string> Ingredients { get; set; }

    public required string Instructions { get; set; }

    public List<string> Pictures { get; set; } = new List<string>();

    public List<string> Videos { get; set; } = new List<string>();

    public int? PreparationTime { get; set; }

    public int? CookingTime { get; set; }

    public int? Servings { get; set; }

    public string Difficulty { get; set; }

    public bool Vegetarian { get; set; }
}
