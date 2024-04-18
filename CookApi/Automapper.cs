using AutoMapper;
using CookApi.DTOs;
using CookApi.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Recipe, RecipeDTO>();
        CreateMap<RecipeDTO, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => MapIngredients(src.Ingredients)))
            .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => MapDifficulty(src.Difficulty)));
    }

    private static List<string> MapIngredients(List<string> ingredients)
    {
        return ingredients.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
    }

    private static RecipeDifficulty? MapDifficulty(string difficulty)
    {
        if (Enum.TryParse(typeof(RecipeDifficulty), difficulty, true, out object? result))
        {
            return (RecipeDifficulty)result;
        }
        return null;
    }
}
