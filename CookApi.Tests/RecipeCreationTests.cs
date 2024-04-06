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
    private readonly string _validInstructions = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat";

    public RecipeCreationTests()
    {
        var webAppFactory = new CustomWebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_ValidMinimumBody()
    {
        var recipe = new
        {
            Name = "Ñóquís con poléntá, queso y ají",
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Instructions = _validInstructions,
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);
        var recipeDTO = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.IsNotNull(recipeDTO, "The recipe returned after creation should not be null");
        Assert.IsTrue(response.StatusCode == HttpStatusCode.Created, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_ValidFullBody()
    {
        var recipe = new
        {
            Name = "Ráviólés con poléntá, queso y ají!",
            Summary = "Un resumen para una receta debe ser conciso",
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Instructions = _validInstructions,
            PreparationTime = 10,
            CookingTime = 25,
            Servings = 2,
            Difficulty = "Easy",
            Vegetarian = true
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);
        var recipeDTO = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.IsNotNull(recipeDTO, "The recipe returned after creation should not be null");
        Assert.IsTrue(response.StatusCode == HttpStatusCode.Created, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_MissingBody()
    {
        var response = await _httpClient.PostAsJsonAsync("/recipes", new { });

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_MissingName()
    {
        var recipe = new
        {
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Instructions = _validInstructions,
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
            Instructions = _validInstructions,
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
            instructions = _validInstructions,
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
            instructions = _validInstructions,
            Ingredients = new List<string> { "ing 1", "ing 2" },
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidTooLongName()
    {
        var recipe = new
        {
            Name = "Extravagant Galactic Unicorn Dream Delight with Twinkling Stardust Sprinkles and Interdimensional Chocolate Fudge Fusion, Served on a Rainbow Comet Cloud Carpet in a Cosmic Crystal Chalice", // Thanks ChatGPT
            instructions = _validInstructions,
            Ingredients = new List<string> { "ing 1", "ing 2" },
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidEmptyInstructions()
    {
        var recipe = new
        {
            Name = "Pizza",
            instructions = " ",
            Ingredients = new List<string> { "ing 1", "ing 2" },
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
            Instructions = _validInstructions,
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Difficulty = "Very easy",
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidTooLongSummary()
    {
        var recipe = new
        {
            Name = "Invalid too long summary recipe name",
            instructions = _validInstructions,
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Summary = "Embark on a journey through the mystical cosmos with our Extravagant Galactic Delight, a mesmerizing fusion of ethereal unicorn dreams, sparkling stardust sprinkles, and decadent chocolate fudge, all served atop a billowing rainbow cloud carpet, transporting you to a realm of unparalleled delight and wonder"
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidPreparationTime()
    {
        var recipe = new
        {
            Name = "Invalid preparation time recipe name",
            instructions = _validInstructions,
            Ingredients = new List<string> { "ing 1", "ing 2" },
            PreparationTime = -23
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidCookingTime()
    {
        var recipe = new
        {
            Name = "Invalid cooking time recipe name",
            instructions = _validInstructions,
            Ingredients = new List<string> { "ing 1", "ing 2" },
            CookingTime = -15
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }

    [TestMethod]
    public async Task Recipes_CreateRecipe_InvalidVegetarian()
    {
        var recipe = new
        {
            Name = "Invalid vegetarian value recipe name",
            instructions = _validInstructions,
            Ingredients = new List<string> { "ing 1", "ing 2" },
            Vegetarian = "false"
        };

        var response = await _httpClient.PostAsJsonAsync("/recipes", recipe);

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"The status code returned should be 400 Bad Request and was {response.StatusCode}");
    }
}
