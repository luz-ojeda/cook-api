using CookApi.Data;
using CookApi.DTOs;
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
    public async Task<ActionResult<List<GroupedIngredientsDTO>>> GetIngredients()
    {
        return await _context.Ingredients
            .OrderBy(i => i.Id)
            .GroupBy(i => i.Name.Substring(0, 1).ToUpper(),
                    (letter, ingredients) => new GroupedIngredientsDTO
                    {
                        Letter = letter,
                        Ingredients = ingredients.OrderBy(i => i.Name).ToList()
                    })
            .OrderBy(i => i.Letter)
            .AsNoTracking()
            .ToListAsync();
    }
}