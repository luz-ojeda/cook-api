using CookApi.Data;
using CookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cook_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController : ControllerBase
{
    private readonly CookApiDbContext _context;
    private readonly ILogger<RecipesController> _logger;

    public RecipesController(CookApiDbContext context, ILogger<RecipesController> logger)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes(
        [FromQuery] int limit = 10,
        [FromQuery] int skip = 0,
        [FromQuery] string? name = null,
        [FromQuery] string[]? ingredients = null
    )
    {
        IQueryable<Recipe> query = _context.Recipes.AsQueryable();

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
        var recipes = await query.Skip(skip).Take(limit).ToListAsync();

        return recipes;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetRecipeById()
    {
        var recipes = await _context.Recipes.ToListAsync();
        return recipes.FirstOrDefault();
    }
}
