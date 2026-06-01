using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailECommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddReplyMessageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyMessage",
                table: "Enquiries",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyMessage",
                table: "Enquiries");
        }
    }
}
