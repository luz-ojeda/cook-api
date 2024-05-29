using CookApi.Models;

namespace CookApi.DTOs;

public class GroupedIngredientsDTO
{
    public string Letter { get; set; }
    public List<Ingredient> Ingredients { get; set; }
}