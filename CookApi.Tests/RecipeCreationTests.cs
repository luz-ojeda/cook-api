using CookApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Json;

namespace CookApi.Tests;

[TestClass]
public class RecipeCreationTests
{
    private readonly HttpClient _httpClient;

    public RecipeCreationTests()
    {
        var webAppFactory = new CustomWebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }

    [TestMethod]
    public async Task Recipes_CreateRecípe_MinimumBody()
    {
        var recipe = new
        {
            Name = "Recipe",
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Instructions = "Lorem ipsum",
            Difficulty = "Medium",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);
        var recipeDTO = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.IsNotNull(recipeDTO, "The recipe returned after creation should not be null");
        Assert.IsTrue(response.StatusCode == HttpStatusCode.Created, "The status code returned should be 201 Created");
    }

    [TestMethod]
    public async Task Recipes_CreateRecípe_MissingName()
    {
        var recipe = new
        {
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Instructions = "Lorem ipsum",
            Difficulty = "Medium",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, "The status code returned should be 400 Bad Request");
    }

    [TestMethod]
    public async Task Recipes_CreateRecípe_MissingIngredients()
    {
        var recipe = new
        {
            Name = "Recipe without ingredients",
            Instructions = "Lorem ipsum",
            Difficulty = "Medium",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, "The status code returned should be 400 Bad Request");
    }

    [TestMethod]
    public async Task Recipes_CreateRecípe_MissingInstructions()
    {
        var recipe = new
        {
            Name = "Recipe without instructions",
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Difficulty = "Medium",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, "The status code returned should be 400 Bad Request");
    }
}
