namespace CookApi.Models;

public class Ingredients
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> Substitutions { get; set; }
}