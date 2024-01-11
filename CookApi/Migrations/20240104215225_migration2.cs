using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cook_api.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive", "und-u-ks-level2,und-u-ks-level2,icu,False");

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "case_insensitive"),
                    Substitutions = table.Column<List<string>>(type: "varchar(50)[]", nullable: false, collation: "case_insensitive")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, collation: "case_insensitive"),
                    Ingredients = table.Column<List<string>>(type: "varchar(200)[]", nullable: false, collation: "case_insensitive"),
                    Instructions = table.Column<string>(type: "text", nullable: false),
                    Pictures = table.Column<List<string>>(type: "varchar(200)[]", nullable: false, defaultValue: new List<string>()),
                    Videos = table.Column<List<string>>(type: "varchar(200)[]", nullable: false, defaultValue: new List<string>()),
                    PreparationTime = table.Column<int>(type: "integer", nullable: true),
                    CookingTime = table.Column<int>(type: "integer", nullable: true),
                    Servings = table.Column<int>(type: "integer", nullable: true),
                    Difficulty = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    Vegetarian = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.CheckConstraint("CK_Recipe_CookingTime", "\"CookingTime\" >= 0 OR \"PreparationTime\" IS NULL");
                    table.CheckConstraint("CK_Recipe_Difficulty", "\"Difficulty\" IN ('Easy', 'Medium', 'Hard') OR \"Difficulty\" IS NULL");
                    table.CheckConstraint("CK_Recipe_Ingredients", "cardinality(\"Ingredients\") > 0");
                    table.CheckConstraint("CK_Recipe_PreparationTime", "\"PreparationTime\" > 0 OR \"PreparationTime\" IS NULL");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Ingredients",
                table: "Recipes",
                column: "Ingredients")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Name",
                table: "Recipes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
