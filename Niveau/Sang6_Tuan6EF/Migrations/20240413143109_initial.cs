using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niveau.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9272de08-18df-4da6-9334-0e098c0987fe", "AQAAAAIAAYagAAAAEApqKfDWebJohPtaimU+PC8FAQYSSjfMlpFi27C03Ab/MtTnHllttlJtLG3NRtInQg==", "c160932f-ab3d-4da1-9eaa-006b609511a4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e337acc-137a-49e7-b9fa-8e741e9792ed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d462ee50-5e1a-4904-9be1-203a7cd81be0", "AQAAAAEAACcQAAAAEFB66wh44CErcr+pcKkbvP8cDpGDl/4HDBVnlpwQNAt+FExHp5p+ajTW+rzXKkM2pA==", "8beebe4e-d687-44ea-920b-2812bcf85b14" });
        }
    }
}
