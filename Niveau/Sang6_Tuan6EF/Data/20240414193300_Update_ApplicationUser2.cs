using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Data
{
    /// <inheritdoc />
    public partial class Update_ApplicationUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "Message", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05bca56c-1b13-47b0-b5a7-af9e90670b35", null, "AQAAAAIAAYagAAAAEM6ktHMKXxP5Vxf1P/uMMDoVeoSSfQu9twVjz8NPqhLRZpv2/gGrNx+6vjTZglgQDA==", "9cfcc14e-07eb-4118-8ea6-b0cf0f36cb9d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d53a516b-16ad-438e-8784-019e916aeca6", "AQAAAAIAAYagAAAAEAUlceaQOpCIMMlBxdhZLWdzbQPFzDRARg1lGNnbO8+ZqK0Sn/4jsEVjeR3KGC22Sw==", "b61583a7-ff08-4203-8cb3-bb1b4a9d7c15" });
        }
    }
}
