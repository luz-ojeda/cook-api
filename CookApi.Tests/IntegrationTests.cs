using CookApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace CookApi.Tests;

[TestClass]
public class IntegrationTests
{
    private readonly HttpClient _httpClient;

    public IntegrationTests()
    {
        var webAppFactory = new CustomWebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipes()
    {
        var response = await _httpClient.GetAsync("/recipes");
        var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>();

        Assert.IsNotNull(recipes, "The list of recipes should not be null");
        Assert.IsTrue(recipes.Count > 0, "The list of recipes should not be empty");
    }

    [TestMethod]
    public async Task Recipes_ReturnPaginatedRecipes()
    {
        var limit = 10;
        var limitQueryParameter = $"limit={limit}";

        var response = await _httpClient.GetAsync("/recipes?" + limitQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>();
        var recipesCount = recipes?.Count;

        Assert.IsTrue(recipesCount == 10, $"Expected {limit} recipes, but got {recipesCount}");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByName()
    {
        var name = "sAlAd";
        var nameQueryParameter = $"name={name}";

        var response = await _httpClient.GetAsync("/recipes?" + nameQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>();

        Assert.IsTrue(
            recipes?.All(r => r.Name.ToLower().Contains("salad")),
            $"Not all recipes contain the string used for searching in their name");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeById()
    {
        var response = await _httpClient.GetAsync("/recipes/:id");
        var recipe = await response.Content.ReadFromJsonAsync<Recipe>();

        Assert.IsNotNull(recipe, "Recipe should not be null");
    }
}
