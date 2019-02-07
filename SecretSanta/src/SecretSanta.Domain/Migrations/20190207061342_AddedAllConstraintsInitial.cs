using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretSanta.Domain.Migrations
{
    public partial class AddedAllConstraintsInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderOfImportance",
                table: "Gifts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name",
                table: "Groups",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_Name",
                table: "Groups");

            migrationBuilder.AlterColumn<int>(
                name: "OrderOfImportance",
                table: "Gifts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
