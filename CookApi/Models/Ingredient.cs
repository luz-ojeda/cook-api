namespace CookApi.Models;

public class Ingredient
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required List<string> Substitutions { get; set; }
}