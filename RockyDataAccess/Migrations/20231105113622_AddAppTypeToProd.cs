using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RockyDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAppTypeToProd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AppTypeId",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Product_AppTypeId",
                table: "Product",
                column: "AppTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationType_AppTypeId",
                table: "Product",
                column: "AppTypeId",
                principalTable: "ApplicationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationType_AppTypeId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_AppTypeId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "AppTypeId",
                table: "Product");
        }
    }
}
