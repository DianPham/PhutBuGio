using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Data
{
    /// <inheritdoc />
    public partial class Sendmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3f97307-5f92-4abf-bd3a-e29b162b1bd9", "AQAAAAIAAYagAAAAEBIWrLVEFwcu9G1KHM3cXA66I3MhxzcESCzKupHrPB+9cmecBz937KRXmc3RTFxw4Q==", "5cf01f06-6776-40bb-9693-b904cdf6f927" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05bca56c-1b13-47b0-b5a7-af9e90670b35", "AQAAAAIAAYagAAAAEM6ktHMKXxP5Vxf1P/uMMDoVeoSSfQu9twVjz8NPqhLRZpv2/gGrNx+6vjTZglgQDA==", "9cfcc14e-07eb-4118-8ea6-b0cf0f36cb9d" });
        }
    }
}
