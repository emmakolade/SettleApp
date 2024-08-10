using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Settle_App.Migrations.SettleAppDB
{
    /// <inheritdoc />
    public partial class allterUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SettleAppFullName",
                schema: "SettleApp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SettleAppUserName",
                schema: "SettleApp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SettleAppFullName",
                schema: "SettleApp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SettleAppUserName",
                schema: "SettleApp",
                table: "AspNetUsers");
        }
    }
}
