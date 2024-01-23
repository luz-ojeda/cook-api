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
        [FromQuery] int skip = 0
    )
    {
        var recipes = await _context.Recipes
            .Skip(skip)
            .Take(limit)
            .ToListAsync();
        return recipes;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetRecipeById()
    {
        var recipes = await _context.Recipes.ToListAsync();
        return recipes.FirstOrDefault();
    }
}
