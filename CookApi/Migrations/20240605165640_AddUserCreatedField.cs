using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCreatedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "Videos",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: true,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldDefaultValue: new List<string>());

            migrationBuilder.AlterColumn<bool>(
                name: "Vegetarian",
                table: "Recipes",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Pictures",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: true,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldDefaultValue: new List<string>());

            migrationBuilder.AddColumn<bool>(
                name: "UserCreated",
                table: "Recipes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "Recipes");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Videos",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldNullable: true,
                oldDefaultValue: new List<string>());

            migrationBuilder.AlterColumn<bool>(
                name: "Vegetarian",
                table: "Recipes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Pictures",
                table: "Recipes",
                type: "varchar(200)[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "varchar(200)[]",
                oldNullable: true,
                oldDefaultValue: new List<string>());
        }
    }
}
