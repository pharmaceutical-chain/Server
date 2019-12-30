using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class CertificatesForMedicineAndBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Certificates",
                table: "Medicines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificates",
                table: "MedicineBatches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Certificates",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Certificates",
                table: "MedicineBatches");
        }
    }
}
