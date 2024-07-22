using AutoMapper;
using CookApi.DTOs;
using CookApi.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Recipe, RecipeDTO>();
        CreateMap<UpdateRecipeDTO, Recipe>()
			.ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
			.ForMember(dest => dest.Summary, opt => opt.Condition(src => src.Summary != null))
			.ForMember(dest => dest.Ingredients, opt => opt.Condition(src => src.Ingredients != null))
			.ForMember(dest => dest.Instructions, opt => opt.Condition(src => src.Instructions != null))
			.ForMember(dest => dest.Pictures, opt => opt.Condition(src => src.Pictures != null))
			.ForMember(dest => dest.Videos, opt => opt.Condition(src => src.Videos != null))
			.ForMember(dest => dest.PreparationTime, opt => opt.Condition(src => src.PreparationTime != null))
			.ForMember(dest => dest.CookingTime, opt => opt.Condition(src => src.CookingTime != null))
			.ForMember(dest => dest.Servings, opt => opt.Condition(src => src.Servings != null))
			.ForMember(dest => dest.Difficulty, opt => opt.Condition(src => src.Difficulty != null))
			.ForMember(dest => dest.Vegetarian, opt => opt.MapFrom(src => src.Vegetarian))
            .ForMember(dest => dest.UserEmail, opt => opt.Condition(src => src.UserEmail != null));
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
