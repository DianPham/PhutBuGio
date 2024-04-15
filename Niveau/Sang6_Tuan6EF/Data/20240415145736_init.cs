using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Data
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cb3e3e46-d3ae-4420-a151-4e98978f11e8", "AQAAAAIAAYagAAAAEG8Qj3/S3nmj+vbJ4zsRpchwCewkneqexIcN1qzbd7mcQt98qaB9MSpn/owC7NmW3w==", "aa48d094-2927-49b7-8a80-f010fde24397" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05bca56c-1b13-47b0-b5a7-af9e90670b35", "AQAAAAIAAYagAAAAEM6ktHMKXxP5Vxf1P/uMMDoVeoSSfQu9twVjz8NPqhLRZpv2/gGrNx+6vjTZglgQDA==", "9cfcc14e-07eb-4118-8ea6-b0cf0f36cb9d" });
        }
    }
}
