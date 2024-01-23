using CookApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;

namespace CookApi.Tests;


[TestClass]
public class GlobalTestInitializer
{
    private static IConfiguration Configuration { get; set; }

    [AssemblyInitialize]
    public async static Task InitialFeed(TestContext testContext)
    {
        Configuration = BuildConfiguration();

        using var serviceProvider = BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CookApiDbContext>();

        await db.Database.EnsureCreatedAsync();

        var sql = File.ReadAllText("insert_recipes.sql");
        await db.Database.ExecuteSqlRawAsync(sql);
    }

    [AssemblyCleanup]
    public static void TearDown()
    {
        Configuration = BuildConfiguration();

        using var serviceProvider = BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CookApiDbContext>();

        db.Database.EnsureDeleted();
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();
    }

    private static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddDbContext<CookApiDbContext>((container, options) =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("TestDB"));
        });

        return services.BuildServiceProvider();
    }
}
