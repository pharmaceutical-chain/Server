using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class Database2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PrimaryAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    RegistrationCode = table.Column<string>(nullable: true),
                    GoodPractices = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    TransactionHash = table.Column<string>(nullable: true),
                    ContractAddress = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    TransactionStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
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
                    DeclaredPrice = table.Column<long>(nullable: false),
                    SubmittedTenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_Tenants_SubmittedTenantId",
                        column: x => x.SubmittedTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CompanyId1 = table.Column<Guid>(nullable: true),
                    ToCompanyId = table.Column<int>(nullable: false),
                    ToCompanyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_Tenants_CompanyId1",
                        column: x => x.CompanyId1,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicineBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionHash = table.Column<string>(nullable: true),
                    ContractAddress = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    TransactionStatus = table.Column<int>(nullable: false),
                    BatchNumber = table.Column<string>(nullable: true),
                    MedicineId = table.Column<Guid>(nullable: false),
                    ManufacturerId = table.Column<Guid>(nullable: false),
                    ManufactureDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<long>(nullable: false),
                    Unit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineBatches_Tenants_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicineBatches_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    EthereumTransactionHash = table.Column<string>(nullable: true),
                    ReceiptId = table.Column<Guid>(nullable: false),
                    DrugName = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    PackageId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Receipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineBatchTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionHash = table.Column<string>(nullable: true),
                    ContractAddress = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    TransactionStatus = table.Column<int>(nullable: false),
                    MedicineBatchId = table.Column<Guid>(nullable: false),
                    FromId = table.Column<Guid>(nullable: false),
                    ToId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineBatchTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineBatchTransfers_Tenants_FromId",
                        column: x => x.FromId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicineBatchTransfers_MedicineBatches_MedicineBatchId",
                        column: x => x.MedicineBatchId,
                        principalTable: "MedicineBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicineBatchTransfers_Tenants_ToId",
                        column: x => x.ToId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatches_ManufacturerId",
                table: "MedicineBatches",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatches_MedicineId",
                table: "MedicineBatches",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatchTransfers_FromId",
                table: "MedicineBatchTransfers",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatchTransfers_MedicineBatchId",
                table: "MedicineBatchTransfers",
                column: "MedicineBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatchTransfers_ToId",
                table: "MedicineBatchTransfers",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_SubmittedTenantId",
                table: "Medicines",
                column: "SubmittedTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CompanyId1",
                table: "Receipts",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiptId",
                table: "Transactions",
                column: "ReceiptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineBatchTransfers");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "MedicineBatches");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
