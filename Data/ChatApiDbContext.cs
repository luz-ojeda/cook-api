using CookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookApi.Data;

public class CookApiContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .Property(c => c.Difficulty)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);

        }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=123;Database=postgres");
}