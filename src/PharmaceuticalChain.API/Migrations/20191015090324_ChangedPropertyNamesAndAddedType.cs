using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class ChangedPropertyNamesAndAddedType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GPCLink",
                table: "Tenants",
                newName: "RegistrationCode");

            migrationBuilder.RenameColumn(
                name: "BRCLink",
                table: "Tenants",
                newName: "GoodPractices");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tenants",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "RegistrationCode",
                table: "Tenants",
                newName: "GPCLink");

            migrationBuilder.RenameColumn(
                name: "GoodPractices",
                table: "Tenants",
                newName: "BRCLink");
        }
    }
}
