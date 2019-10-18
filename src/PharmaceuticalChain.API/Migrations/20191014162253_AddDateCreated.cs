using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class AddDateCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Tenants",
                newName: "PrimaryAddress");

            migrationBuilder.AddColumn<string>(
                name: "DateCreated",
                table: "Tenants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "PrimaryAddress",
                table: "Tenants",
                newName: "Address");
        }
    }
}
