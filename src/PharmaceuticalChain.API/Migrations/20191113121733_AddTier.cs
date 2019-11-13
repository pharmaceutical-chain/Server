using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class AddTier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Tier",
                table: "MedicineBatchTransfers",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tier",
                table: "MedicineBatchTransfers");
        }
    }
}
