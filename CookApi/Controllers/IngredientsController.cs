using CookApi.Data;
using CookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cook_api.Controllers;

[ApiController]
[Route("[controller]")]
public class IngredientsController(
    CookApiDbContext context,
    ILogger<RecipesController> logger) : ControllerBase
{
    private readonly CookApiDbContext _context = context;
    private readonly ILogger<RecipesController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<ActionResult<PaginatedList<Ingredient>>> GetIngredients(
    [FromQuery] int limit = 10,
    [FromQuery] int page = 1,
    [FromQuery] string? name = null
)
    {
        IQueryable<Ingredient> query = _context.Ingredients.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(ingredient => ingredient.Name.ToLower().Contains(name.ToLower()));
        }

        var ingredients = query
            .OrderBy(r => r.Id)
            .AsNoTracking();

        return await PaginatedList<Ingredient>.CreateAsync(ingredients, page, limit);
    }

}