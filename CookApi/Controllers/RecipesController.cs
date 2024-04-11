using CookApi.Data;
using CookApi.DTOs;
using CookApi.Models;
using CookApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cook_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController(
    CookApiDbContext context,
    ILogger<RecipesController> logger,
    RecipesService recipesService) : ControllerBase
{
    private readonly CookApiDbContext _context = context;
    private readonly ILogger<RecipesController> _logger = logger;
    private readonly RecipesService _recipesService = recipesService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedList<RecipeDTO>>> GetRecipes(
        [FromQuery] int limit = 10,
        [FromQuery] int page = 1,
        [FromQuery] string? name = null,
        [FromQuery] string[]? ingredients = null,
        [FromQuery] RecipeDifficulty?[]? difficulty = null,
        [FromQuery] string[]? ids = null,
        [FromQuery] bool onlyVegetarian = false
    )
    {
        IQueryable<Recipe> query = _context.Recipes.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(recipe => recipe.Name.ToLower().Contains(name.ToLower()));
        }

        if (ingredients != null && ingredients.Length != 0)
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
            var difficultyFilter = difficulty.ToArray();
            query = query.Where(recipe => difficultyFilter.Contains(recipe.Difficulty));
        }

        if (ids != null && ids.Length != 0)
        {
            foreach (string id in ids)
            {
                query = query.Where(recipe => ids.Contains(recipe.Id.ToString()));
            }
        }

        if (onlyVegetarian)
        {
            query = query.Where(recipe => recipe.Vegetarian ?? false);
        }

        var recipes = query
            .OrderBy(r => r.Id)
            .Select(recipe => RecipeToDTO(recipe))
            .AsNoTracking();

        return await PaginatedList<RecipeDTO>.CreateAsync(recipes, page, limit);
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeDTO>> GetRecipeByName(string name)
    {
        var cleanName = name.Replace("-", " ").ToLower();
        var recipe = await _context.Recipes
                        .AsNoTracking()
                        .Where(r => r.Name.ToLower().Replace("-", " ") == cleanName)
                        .FirstOrDefaultAsync();

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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRecipeById(Guid id)
    {
        var recipe = await _context.Recipes.FindAsync(id);

        if (recipe == null)
        {
            return NotFound();
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RecipeDTO>> PostRecipe(RecipeDTO recipeDTO)
    {
        var recipe = await _recipesService.CreateRecipe(recipeDTO);

        return Created(nameof(GetRecipeById), RecipeToDTO(recipe));
    }

    private static RecipeDTO RecipeToDTO(Recipe recipe) =>
       new()
       {
           Id = recipe.Id,
           Name = recipe.Name,
           Summary = recipe.Summary,
           Ingredients = recipe.Ingredients,
           Instructions = recipe.Instructions,
           Pictures = recipe.Pictures,
           Videos = recipe.Videos,
           PreparationTime = recipe.PreparationTime,
           CookingTime = recipe.CookingTime,
           Servings = recipe.Servings,
           Difficulty = recipe.Difficulty?.ToString(),
           Vegetarian = recipe.Vegetarian ?? false
       };
}
