using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailECommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddUsedDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsedDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscountId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedDiscounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsedDiscounts_DiscountId",
                table: "UsedDiscounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedDiscounts_UserId_DiscountId",
                table: "UsedDiscounts",
                columns: new[] { "UserId", "DiscountId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsedDiscounts");
        }
    }
}
