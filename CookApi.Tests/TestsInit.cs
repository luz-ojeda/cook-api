using CookApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;

namespace CookApi.Tests;


[TestClass]
public class GlobalTestInitializer
{
    [AssemblyInitialize]
    public static void InitialFeed(TestContext testContext)
    {
    }

    [AssemblyCleanup]
    public static void TearDown()
    {
        var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

        var services = new ServiceCollection();
        services.AddDbContext<CookApiDbContext>((container, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("TestDB"));
            });

        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CookApiDbContext>();
            db.Database.EnsureDeleted();
        }
    }
}
