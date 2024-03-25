using CookApi.DTOs;
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
        var paginatedList = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(paginatedList, "The list of recipes should not be null");
        Assert.IsTrue(paginatedList.Data.Count > 0, "The list of recipes should not be empty");
    }

    [TestMethod]
    public async Task Recipes_ReturnPaginatedRecipes()
    {
        var limit = 5;
        var limitQueryParameter = $"limit={limit}";

        var response = await _httpClient.GetAsync("/recipes?" + limitQueryParameter);
        var paginatedList = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(paginatedList, "The list of recipes should not be null");
        Assert.IsTrue(
            paginatedList.Data.Count == limit,
            $"Expected {limit} recipes, but got {paginatedList.Data.Count}");
        Assert.IsTrue(
            paginatedList.Pagination.PageSize == limit,
            $"Expected {limit} page size value in Pagination but got {paginatedList.Pagination.PageSize}");
    }

    [TestMethod]
    public async Task Recipes_ReturnEmptyListIfPastLastPage()
    {
        var firstResponse = await _httpClient.GetAsync("/recipes");
        var firstPaginatedList = await firstResponse.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(firstPaginatedList, "The list of recipes should not be null");

        var page = firstPaginatedList.Pagination.TotalPages + 1;
        var pageQueryParameter = $"page={page}";

        var secondResponse = await _httpClient.GetAsync("/recipes?" + pageQueryParameter);
        var secondPaginatedList = await secondResponse.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(secondPaginatedList, "The list of recipes should not be null");
        Assert.IsTrue(
            secondPaginatedList.Data.Count == 0,
            $"Expected 0 recipes, but got {secondPaginatedList.Data.Count}");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByName()
    {
        var name = "sAlAd";
        var nameQueryParameter = $"name={name}";

        var response = await _httpClient.GetAsync("/recipes?" + nameQueryParameter);
        var paginatedList = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(paginatedList, "The list of recipes should not be null");
        Assert.IsTrue(
            paginatedList.Data.All(r => r.Name.ToLower().Contains("salad")),
            "Not all recipes contain the string used for searching in their name");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByIngredient()
    {
        List<string> ingredientsToBeSearchedFor = ["cHEeSe", "PaStA"];
        var ingredientsQueryParameter = $"ingredients={ingredientsToBeSearchedFor[0]}&ingredients={ingredientsToBeSearchedFor[1]}";

        var response = await _httpClient.GetAsync("/recipes?" + ingredientsQueryParameter);
        var paginatedList = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(paginatedList, "The list of recipes should not be null");
        Assert.IsTrue(paginatedList.Data.Count > 0, "Zero recipes were returned");
        Assert.IsTrue(
            paginatedList.Data.All(
                r => ingredientsToBeSearchedFor.Any(
                    i => r.Ingredients.Any(
                        recipeIngredient => recipeIngredient.Contains(i.ToLower())))),
            "Not all recipes contain at least one of the specified ingredients");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByDifficulty()
    {
        RecipeDifficulty difficulty = RecipeDifficulty.Easy;
        var difficultyQueryParameter = $"difficulty={difficulty}";

        var response = await _httpClient.GetAsync("/recipes?" + difficultyQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(recipes, "The list of recipes should not be null");
        Assert.IsTrue(recipes.Data.Count > 0, "Zero recipes were returned");
        Assert.IsTrue(recipes.Data.All(r => r.Difficulty == "Easy"));
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByDifficulty_UsingInvalidQueryParameter()
    {
        string difficulty = "impossible";
        var difficultyQueryParameter = $"difficulty={difficulty}";

        var response = await _httpClient.GetAsync("/recipes?" + difficultyQueryParameter);

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task Recipes_ReturnOnlyVegetarianRecipes()
    {
        var onlyVegetarianQueryParameter = $"onlyVegetarian=true";

        var response = await _httpClient.GetAsync("/recipes?" + onlyVegetarianQueryParameter);
        var paginatedList = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(paginatedList, "The list of recipes should not be null");
        Assert.IsTrue(
            paginatedList.Data.All(r => r.Vegetarian == true),
            "Not all recipes returned were vegetarian");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByListOfIds()
    {
        List<Guid> idsToSeachFor = [
            new Guid("f6f3e96b-8583-4cda-8fc6-9f260fb6bc09"),
            new Guid("c7393218-9029-4990-818f-d4f72d53e793"),
            new Guid("54a8e08b-8c88-4d67-8aa0-70330c4a2156")];
        var idsQueryParameter = $"ids={idsToSeachFor[0]}&ids={idsToSeachFor[1]}&ids={idsToSeachFor[2]}";

        var response = await _httpClient.GetAsync("/recipes?" + idsQueryParameter);
        var paginatedList = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(paginatedList, "The list of recipes should not be null");
        Assert.IsTrue(paginatedList.Data.Count > 0, "Zero recipes were returned");
        Assert.IsTrue(
            paginatedList.Data.All(
                recipe => idsToSeachFor.Any(
                    id => id == recipe.Id
                ))); // For all recipes returned there is at least one matching Id in the list of array of Ids used in the query
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByListOfIds_NotFound()
    {
        List<Guid> idsToSeachFor = [
            new Guid("a6f3e96b-8583-4cda-8fc6-9f260fb6bc09"),
            new Guid("a7393218-9029-4990-818f-d4f72d53e793"),
            new Guid("a4a8e08b-8c88-4d67-8aa0-70330c4a2156")];
        var idsQueryParameter = $"ids={idsToSeachFor[0]}&ids={idsToSeachFor[1]}&ids={idsToSeachFor[2]}";

        var response = await _httpClient.GetAsync("/recipes?" + idsQueryParameter);
        var recipes = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(recipes, "The list of recipes should not be null");
        Assert.IsTrue(recipes.Data.Count == 0, "More than one recipe were returned");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesPagedAndFiltered()
    {
        var name = "salmon";
        var nameQueryParameter = $"name={name}";

        RecipeDifficulty difficulty = RecipeDifficulty.Easy;
        var difficultyQueryParameter = $"difficulty={difficulty}";

        List<string> ingredientsToBeSearchedFor = ["broccoli", "quinoa"];
        var ingredientsQueryParameter = $"ingredients={ingredientsToBeSearchedFor[0]}&ingredients={ingredientsToBeSearchedFor[1]}";

        var pageQueryParameter = "page=1";
        var limitQueryParameter = "limit=3";

        var response = await _httpClient.GetAsync(
            $"/recipes?{difficultyQueryParameter}&{ingredientsQueryParameter}&{nameQueryParameter}&{pageQueryParameter}&{limitQueryParameter}");
        var paginatedList = await response.Content.ReadFromJsonAsync<PaginatedList<RecipeDTO>>();

        Assert.IsNotNull(paginatedList, "The list of recipes should not be null");
        Assert.IsTrue(paginatedList.Data.Count != 0, "Zero recipes were returned");
        Assert.IsTrue(
            paginatedList.Data.All(r => r.Name.ToLower().Contains(name)),
            "Not all recipes contain the string used for searching in their name");
        Assert.IsTrue(
            paginatedList.Data.All(
                r => ingredientsToBeSearchedFor.Any(
                    i => r.Ingredients.Any(
                        recipeIngredient => recipeIngredient.Contains(i.ToLower())))),
            "Not all recipes contain at least one of the specified ingredients");
        Assert.IsTrue(paginatedList.Data.All(r => r.Difficulty == "Easy"));
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeByName()
    {
        string name = new("Vegetable lasaGnA");
        var response = await _httpClient.GetAsync("/recipes/name/" + name);
        var paginatedList = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.IsNotNull(paginatedList, "Recipe should not be null");
        Assert.IsTrue(paginatedList.Name.ToLower() == name.ToLower(), "Recipe's nameis not the same as provided in the query path");
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipesByName_NotFound()
    {
        string name = new("pastafrola");
        var response = await _httpClient.GetAsync("/recipes/name/" + name);

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Recipes_ReturnRecipeById()
    {
        Guid id = new("f6f3e96b-8583-4cda-8fc6-9f260fb6bc09");
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
