using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infraestructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class IDCardNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IDCard",
                schema: "Identity",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "IDCard",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IDCard",
                schema: "Identity",
                table: "Users",
                column: "IDCard",
                unique: true,
                filter: "[IDCard] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IDCard",
                schema: "Identity",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "IDCard",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IDCard",
                schema: "Identity",
                table: "Users",
                column: "IDCard",
                unique: true);
        }
    }
}
