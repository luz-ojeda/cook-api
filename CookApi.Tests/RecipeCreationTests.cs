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
    public async Task Recipes_CreateRecipe_MinimumBody()
    {
        var recipe = new
        {
            Name = "Recipe",
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Instructions = "Lorem ipsum",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);
        var recipeDTO = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.IsNotNull(recipeDTO, "The recipe returned after creation should not be null");
        Assert.IsTrue(response.StatusCode == HttpStatusCode.Created, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_MissingName()
    {
        var recipe = new
        {
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Instructions = "Lorem ipsum",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_MissingIngredients()
    {
        var recipe = new
        {
            Name = "Recipe without ingredients",
            Instructions = "Lorem ipsum",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_MissingInstructions()
    {
        var recipe = new
        {
            Name = "Recipe without instructions",
            Ingredients = new List<string> { "ing 1", "ing 2" },
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidSymbolsName()
    {
        var recipe = new
        {
            Name = "Inv%lid r&i()3¬",
            Instructions = "Instructions",
            Ingredients = new List<string> { "ing 1", "ing 2" },
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidEmptyName()
    {
        var recipe = new
        {
            Name = " ",
            Instructions = "Instructions",
            Ingredients = new List<string> { "ing 1", "ing 2" },
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidEmptyIngredients()
    {
        var recipe = new
        {
            Name = "Invalid empty ingredients recipe Name",
            Instructions = " ",
            Ingredients = new List<string> { " ", " " },
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidDifficulty()
    {
        var recipe = new
        {
            Name = "Invalid difficulty recipe name",
            Instructions = " ",
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Difficulty = "Super duper easy",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }
}
