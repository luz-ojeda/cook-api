using CookApi.Data;
using CookApi.DTOs;
using CookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookApi.Services;

public class RecipesService(CookApiDbContext context)
{
    private readonly CookApiDbContext _context = context;

    public async Task<Recipe> CreateRecipe(RecipeDTO recipeDTO)
    {
        // if (recipeDTO.Difficulty != null && !Enum.IsDefined(typeof(RecipeDifficulty), recipeDTO.Difficulty))
        // {
        //     return BadRequest();
        // }

        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = recipeDTO.Name,
            Summary = recipeDTO.Summary,
            Ingredients = recipeDTO.Ingredients,
            Instructions = recipeDTO.Instructions,
            Pictures = recipeDTO.Pictures,
            Videos = recipeDTO.Videos,
            PreparationTime = recipeDTO.PreparationTime,
            CookingTime = recipeDTO.CookingTime,
            Servings = recipeDTO.Servings,
            Difficulty = recipeDTO.Difficulty != null ? (RecipeDifficulty)Enum.Parse(typeof(RecipeDifficulty), recipeDTO.Difficulty) : null,
            Vegetarian = recipeDTO.Vegetarian
        };

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        return recipe;
    }
}