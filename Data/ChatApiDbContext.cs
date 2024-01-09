using CookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookApi.Data;

public class CookApiContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }

    public DbSet<Ingredient> Ingredients { get; set; }

    public CookApiContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCollation("case_insensitive", provider: "icu", locale: "und-u-ks-level2", deterministic: false);

            /* Indexes */
            modelBuilder.Entity<Recipe>()
                .HasIndex(r => new { r.Name })
                .IsUnique();

            modelBuilder.Entity<Recipe>()
                .HasIndex(r => new { r.Ingredients })
                .HasMethod("gin");

            modelBuilder.Entity<Ingredient>()
                .HasIndex(r => new { r.Name })
                .IsUnique();

            /* Properties */
            modelBuilder.Entity<Recipe>()
                .Property(r => r.Name)
                .UseCollation("case_insensitive")
                .HasMaxLength(150);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Ingredients)
                .UseCollation("case_insensitive")
                .HasColumnType("varchar(200)[]");

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Pictures)
                .HasColumnType("varchar(200)[]")
                .HasDefaultValue(new List<string>());

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Videos)
                .HasColumnType("varchar(200)[]")
                .HasDefaultValue(new List<string>());

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Difficulty)
                .HasConversion(
                    v => v.ToString(),
                    v => (RecipeDifficulty)Enum.Parse(typeof(RecipeDifficulty), v))
                .HasMaxLength(6);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Vegetarian)
                .HasDefaultValue(false);

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Name)
                .UseCollation("case_insensitive")
                .HasMaxLength(50);

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Substitutions)
                .UseCollation("case_insensitive")
                .HasColumnType("varchar(50)[]");

            /* Constraints */
            modelBuilder.Entity<Recipe>()
                .ToTable(r => r.HasCheckConstraint("CK_Recipe_Ingredients", "cardinality(\"Ingredients\") > 0"));

            modelBuilder.Entity<Recipe>()
                .ToTable(r => r.HasCheckConstraint("CK_Recipe_PreparationTime", "\"PreparationTime\" > 0 OR \"PreparationTime\" IS NULL"));

            modelBuilder.Entity<Recipe>()
                .ToTable(r => r.HasCheckConstraint("CK_Recipe_CookingTime", "\"CookingTime\" >= 0 OR \"PreparationTime\" IS NULL"));

            modelBuilder.Entity<Recipe>()
                .ToTable(r => r.HasCheckConstraint("CK_Recipe_Difficulty", "\"Difficulty\" IN ('Easy', 'Medium', 'Hard') OR \"Difficulty\" IS NULL"));

            base.OnModelCreating(modelBuilder);
        }
}