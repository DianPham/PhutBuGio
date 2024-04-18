using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Data
{
    /// <inheritdoc />
    public partial class Update_Ỏder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "016e4022-3e5c-4b44-9297-568d1c1d02cd", "AQAAAAIAAYagAAAAEISzikpB5GU3Ct7KhizdHgmOmeiA/i3sOjgb1eQwQ5vZTSFI6/O+p7jjk7p6cJ0UJA==", "964b49af-e61d-4be9-b431-6c217c692e2f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "daa4d004-dd1b-46f6-ac37-8f5fc485abd2", "AQAAAAIAAYagAAAAEENUGe8/cET5S3EOi1Co03E40d91CdcUHtXus7vBNpy8EibrewE0HFeivJvwGHTVLw==", "62be947d-a036-4fa4-a73d-8b6b9f4bfe22" });
        }
    }
}
