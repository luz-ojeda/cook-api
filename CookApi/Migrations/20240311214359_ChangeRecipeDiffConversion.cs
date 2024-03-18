using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRecipeDiffConversion : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Recipes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Recipes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

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
