using CookApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http.Json;

namespace CookApi.Tests
{
    [TestClass]
    public class ApiTests
    {
        private readonly HttpClient _httpClient;

        public ApiTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [TestMethod]
        public async Task Recipes_ReturnRecipes()
        {
            var response = await _httpClient.GetAsync("/recipes");
            var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>();

            Assert.IsNotNull(recipes, "The list of recipes should not be null");
            Assert.IsTrue(recipes.Any(), "The list of recipes should not be empty");
        }

        [TestMethod]
        public async Task Recipes_ReturnRecipeById()
        {
            var response = await _httpClient.GetAsync("/recipes/:id");
            var recipe = await response.Content.ReadFromJsonAsync<Recipe>();

            Assert.IsNotNull(recipe, "Recipe should not be null");
        }
    }
}