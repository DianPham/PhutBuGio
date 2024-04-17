using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Data
{
    /// <inheritdoc />
    public partial class Update_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "daa4d004-dd1b-46f6-ac37-8f5fc485abd2", "AQAAAAIAAYagAAAAEENUGe8/cET5S3EOi1Co03E40d91CdcUHtXus7vBNpy8EibrewE0HFeivJvwGHTVLw==", "62be947d-a036-4fa4-a73d-8b6b9f4bfe22" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3f97307-5f92-4abf-bd3a-e29b162b1bd9", "AQAAAAIAAYagAAAAEBIWrLVEFwcu9G1KHM3cXA66I3MhxzcESCzKupHrPB+9cmecBz937KRXmc3RTFxw4Q==", "5cf01f06-6776-40bb-9693-b904cdf6f927" });
        }
    }
}
