﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PharmaceuticalChain.API.Models.Database;

namespace PharmaceuticalChain.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.MedicineBatch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BatchNumber");

                    b.Property<string>("CommercialName");

                    b.Property<string>("ContractAddress");

                    b.Property<DateTime>("DateCreated");

                    b.Property<long>("DeclaredPrice");

                    b.Property<string>("DosageForm");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<string>("IngredientConcentration");

                    b.Property<bool>("IsPrescriptionMedicine");

                    b.Property<DateTime>("ManufactureDate");

                    b.Property<string>("PackingSpecification");

                    b.Property<long>("Quantity");

                    b.Property<string>("RegistrationCode");

                    b.Property<string>("TransactionHash");

                    b.Property<int>("TransactionStatus");

                    b.HasKey("Id");

                    b.ToTable("MedicineBatches");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Receipt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<Guid?>("CompanyId1");

                    b.Property<int>("ToCompanyId");

                    b.Property<string>("ToCompanyName");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId1");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContractAddress");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("GoodPractices");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PrimaryAddress");

                    b.Property<string>("RegistrationCode");

                    b.Property<string>("TaxCode");

                    b.Property<string>("TransactionHash");

                    b.Property<int>("TransactionStatus");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Transaction", b =>
                {
                    b.Property<long>("Id");

                    b.Property<long>("Amount");

                    b.Property<string>("DrugName");

                    b.Property<string>("EthereumTransactionHash");

                    b.Property<string>("PackageId");

                    b.Property<Guid>("ReceiptId");

                    b.HasKey("Id");

                    b.HasIndex("ReceiptId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Receipt", b =>
                {
                    b.HasOne("PharmaceuticalChain.API.Models.Database.Tenant", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId1");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Transaction", b =>
                {
                    b.HasOne("PharmaceuticalChain.API.Models.Database.Receipt", "Receipt")
                        .WithMany("Transactions")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
