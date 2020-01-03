using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class FixName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IApprovedByAdmin",
                table: "MedicineBatches",
                newName: "IsApprovedByAdmin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsApprovedByAdmin",
                table: "MedicineBatches",
                newName: "IApprovedByAdmin");
        }
    }
}
