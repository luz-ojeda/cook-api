using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using CookApi.Data;
using CookApi.DTOs;
using CookApi.Models;
using AutoMapper;

namespace CookApi.Services;

public class RecipesService(CookApiDbContext context, IMapper mapper)
{
    private readonly CookApiDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Recipe> CreateRecipe(RecipeDTO recipeDTO)
    {
        var recipe = _mapper.Map<Recipe>(recipeDTO);

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