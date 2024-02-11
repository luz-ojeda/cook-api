using CookApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeDTO>>();

        Assert.IsNotNull(recipes, "The list of recipes should not be null");
        Assert.IsTrue(recipes.Count > 0, "The list of recipes should not be empty");
    }

    [TestMethod]
    public async Task Recipes_ReturnPaginatedRecipes()
    {
        var limit = 5;
        var limitQueryParameter = $"limit={limit}";

        var response = await _httpClient.GetAsync("/recipes?" + limitQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeDTO>>();
        var recipesCount = recipes?.Count;

        Assert.IsTrue(recipesCount == limit, $"Expected {limit} recipes, but got {recipesCount}");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByName()
    {
        var name = "sAlAd";
        var nameQueryParameter = $"name={name}";

        var response = await _httpClient.GetAsync("/recipes?" + nameQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeDTO>>();

        Assert.IsTrue(
            recipes?.All(r => r.Name.ToLower().Contains("salad")),
            "Not all recipes contain the string used for searching in their name");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByIngredient()
    {
        List<string> ingredientsToBeSearchedFor = ["cHEeSe", "PaStA"];
        var ingredientsQueryParameter = $"ingredients={ingredientsToBeSearchedFor[0]}&ingredients={ingredientsToBeSearchedFor[1]}";

        var response = await _httpClient.GetAsync("/recipes?" + ingredientsQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeDTO>>();

        Assert.IsTrue(recipes?.Count > 0, "Zero recipes were returned");
        Assert.IsTrue(
            recipes?.All(
                r => ingredientsToBeSearchedFor.Any(
                    i => r.Ingredients.Any(
                        recipeIngredient => recipeIngredient.Contains(i.ToLower())))),
            "Not all recipes contain at least one of the specified ingredients");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeByDifficulty()
    {
        RecipeDifficulty difficulty = RecipeDifficulty.Easy;
        var difficultyQueryParameter = $"difficulty={difficulty}";

        var response = await _httpClient.GetAsync("/recipes?" + difficultyQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeDTO>>();

        Assert.IsTrue(recipes?.Count > 0, "Zero recipes were returned");
        Assert.IsTrue(recipes.All(r => r.Difficulty == "Easy"));
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeByDifficulty_UsingInvalidQueryParameter()
    {
        string difficulty = "impossible";
        var difficultyQueryParameter = $"difficulty={difficulty}";

        var response = await _httpClient.GetAsync("/recipes?" + difficultyQueryParameter);

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeByName()
    {
        string name = new("Vegetable lasaGnA");
        var response = await _httpClient.GetAsync("/recipes/name/" + name);
        var recipe = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.IsNotNull(recipe, "Recipe should not be null");
        Assert.IsTrue(recipe.Name.ToLower() == name.ToLower(), "Recipe's nameis not the same as provided in the query path");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeByName_NotFound()
    {
        string name = new("pastafrola");
        var response = await _httpClient.GetAsync("/recipes/name/" + name);

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeById()
    {
        Guid id = new ("f6f3e96b-8583-4cda-8fc6-9f260fb6bc09");
        var response = await _httpClient.GetAsync("/recipes/" + id);
        var recipe = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.IsNotNull(recipe, "Recipe should not be null");
        Assert.IsTrue(recipe.Id == id, "Recipe's id is not the same as provided in the query path");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeById_NotFound()
    {
        Guid id = new("f6f3e96b-8583-4cda-8fc6-1f234fb5bc67");
        var response = await _httpClient.GetAsync("/recipes/" + id);

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
    }
}
