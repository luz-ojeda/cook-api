namespace CookApi.Models;

public class Ingredient
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<string> Substitutions { get; set; }
}