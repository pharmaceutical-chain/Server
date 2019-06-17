﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PharmaceuticalChain.API.Models.Database;

namespace PharmaceuticalChain.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190617190506_EditReceipt")]
    partial class EditReceipt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Company", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Receipt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("ToCompanyId");

                    b.Property<string>("ToCompanyName");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Transaction", b =>
                {
                    b.Property<long>("Id");

                    b.Property<string>("EthereumTransactionHash");

                    b.Property<Guid>("ReceiptId");

                    b.HasKey("Id");

                    b.HasIndex("ReceiptId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("PharmaceuticalChain.API.Models.Database.Receipt", b =>
                {
                    b.HasOne("PharmaceuticalChain.API.Models.Database.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
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
