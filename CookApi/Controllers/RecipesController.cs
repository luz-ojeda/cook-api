using CookApi.Data;
using CookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cook_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController(CookApiDbContext context, ILogger<RecipesController> logger) : ControllerBase
{
    private readonly CookApiDbContext _context = context;
    private readonly ILogger<RecipesController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RecipeDTO>>> GetRecipes(
        [FromQuery] int limit = 10,
        [FromQuery] int skip = 0,
        [FromQuery] string? name = null,
        [FromQuery] string[]? ingredients = null,
        [FromQuery] RecipeDifficulty? difficulty = null
    )
    {
        IQueryable<Recipe> query = _context.Recipes.AsQueryable().AsNoTracking();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(recipe => recipe.Name.ToLower().Contains(name.ToLower()));
        }

        if (ingredients != null && ingredients?.Length != 0)
        {
            var lowerCaseIngredients = ingredients.Select(i => i.ToLower()).ToArray();

            // The following produces a query that retrieves recipes that have ALL the ingredientes used in the
            // query parameter in its recipes
            //
            // For example: using "cheese" and "pasta" retrieves all the recipes that at least have those _two_
            // ingredients in their Ingredients property
            foreach (string ingredient in lowerCaseIngredients)
            {
                query = query.Where(
                    recipe => recipe.Ingredients.Any(i => i.Contains(ingredient))
                );
            }
        }

        if (difficulty != null)
        {
            query = query.Where(recipe => recipe.Difficulty == difficulty);
        }

        var recipes = await query
            .Skip(skip)
            .Take(limit)
            .Select(recipe => RecipeToDTO(recipe))
            .ToListAsync();

        return recipes;
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeDTO>> GetRecipeByName(string name)
    {
        var recipe = await _context.Recipes.Where(r => r.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();

        if (recipe == null)
        {
            return NotFound();
        }

        return RecipeToDTO(recipe);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeDTO>> GetRecipeById(Guid id)
    {
        var recipe = await _context.Recipes.FindAsync(id);

        if (recipe == null)
        {
            return NotFound();
        }

        return RecipeToDTO(recipe);
    }


    private static RecipeDTO RecipeToDTO(Recipe recipe) =>
       new RecipeDTO
       {
           Id = recipe.Id,
           Name = recipe.Name,
           Ingredients = recipe.Ingredients,
           Instructions = recipe.Instructions,
           Pictures = recipe.Pictures,
           Videos = recipe.Videos,
           PreparationTime = recipe.PreparationTime,
           CookingTime = recipe.CookingTime,
           Servings = recipe.Servings,
           Difficulty = recipe.Difficulty?.ToString(),
           Vegetarian = recipe.Vegetarian
       };
}
