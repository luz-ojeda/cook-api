using System.Data;
using CookApi.Data;
using CookApi.DTOs;
using CookApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CookApi.Services;

public class RecipesService(CookApiDbContext context)
{
    private readonly CookApiDbContext _context = context;

    public async Task<Recipe> CreateRecipe(RecipeDTO recipeDTO)
    {
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

        try
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException is NpgsqlException { SqlState: "23505" })
        {
            throw new DuplicateNameException($"The recipe with name {recipeDTO.Name} already exists.");
        }

        return recipe;
    }
}