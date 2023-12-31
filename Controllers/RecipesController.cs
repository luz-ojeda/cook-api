using CookApi.Data;
using CookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cook_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController : ControllerBase
{
    private readonly CookApiContext _context;
    private readonly ILogger<RecipesController> _logger;

    public RecipesController(CookApiContext context, ILogger<RecipesController> logger)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
    {
        var recipes = await _context.Recipes.ToListAsync();
        return recipes;
    }
}
