using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyStoreProject.Services.Payment.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PAYMENTS_OrderId",
                table: "PAYMENTS",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PAYMENTS_OrderId",
                table: "PAYMENTS");
        }
    }
}
