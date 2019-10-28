using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class AddMedicineBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicineBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionHash = table.Column<string>(nullable: true),
                    ContractAddress = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    TransactionStatus = table.Column<int>(nullable: false),
                    CommercialName = table.Column<string>(nullable: true),
                    RegistrationCode = table.Column<string>(nullable: true),
                    BatchNumber = table.Column<string>(nullable: true),
                    IsPrescriptionMedicine = table.Column<bool>(nullable: false),
                    DosageForm = table.Column<string>(nullable: true),
                    IngredientConcentration = table.Column<string>(nullable: true),
                    PackingSpecification = table.Column<string>(nullable: true),
                    Quantity = table.Column<long>(nullable: false),
                    DeclaredPrice = table.Column<long>(nullable: false),
                    ManufactureDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineBatches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineBatches");
        }
    }
}
