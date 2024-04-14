using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Data
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "44919be3-e616-42ad-ba8c-6583ca73d4ca", "AQAAAAIAAYagAAAAEInHxYxEOwCklP3DHZH/GQlnTgbU9U/zOwkA30+1hq1wJORIuTeXCFHIGpdFMYN48Q==", null, "2c1c39f7-27d1-48e0-94ac-4f82ae25cc67" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "d462ee50-5e1a-4904-9be1-203a7cd81be0", "AQAAAAEAACcQAAAAEFB66wh44CErcr+pcKkbvP8cDpGDl/4HDBVnlpwQNAt+FExHp5p+ajTW+rzXKkM2pA==", null, "8beebe4e-d687-44ea-920b-2812bcf85b14" });
        }
    }
}
