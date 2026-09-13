using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyStoreProject.Services.Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_row_version : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "RESERVATIONS",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)");

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "INVENTORY",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "RESERVATIONS");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "INVENTORY");
        }
    }
}
