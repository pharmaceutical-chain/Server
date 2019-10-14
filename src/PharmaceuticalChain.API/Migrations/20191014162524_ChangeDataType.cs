using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaceuticalChain.API.Migrations
{
    public partial class ChangeDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Tenants",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateCreated",
                table: "Tenants",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
