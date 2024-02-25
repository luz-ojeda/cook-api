using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSummaryToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "Videos",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldDefaultValue: new List<string>());

            migrationBuilder.AlterColumn<List<string>>(
                name: "Pictures",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldDefaultValue: new List<string>());

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Recipes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Recipes");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Videos",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldDefaultValue: new List<string>());

            migrationBuilder.AlterColumn<List<string>>(
                name: "Pictures",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldDefaultValue: new List<string>());
        }
    }
}
