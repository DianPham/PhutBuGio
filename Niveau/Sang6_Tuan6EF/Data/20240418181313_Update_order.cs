using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Data
{
    /// <inheritdoc />
    public partial class Update_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cb21249a-2860-4dcb-a3f8-eb9c99b83705", "AQAAAAIAAYagAAAAEIc/ij70AEmGmLSButoIr41Eb0NZwZ5ITzVXrjnZmyrfaQVOiOJzCRfRLlXfP3Rn2Q==", "7283ac6a-295c-48a3-831b-8219140aba1f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "daa4d004-dd1b-46f6-ac37-8f5fc485abd2", "AQAAAAIAAYagAAAAEENUGe8/cET5S3EOi1Co03E40d91CdcUHtXus7vBNpy8EibrewE0HFeivJvwGHTVLw==", "62be947d-a036-4fa4-a73d-8b6b9f4bfe22" });
        }
    }
}
