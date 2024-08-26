using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class ErrorInDetailsTablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_Headers_CartHeaderId",
                table: "Descriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Descriptions",
                table: "Descriptions");

            migrationBuilder.RenameTable(
                name: "Descriptions",
                newName: "Details");

            migrationBuilder.RenameIndex(
                name: "IX_Descriptions_CartHeaderId",
                table: "Details",
                newName: "IX_Details_CartHeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Details",
                table: "Details",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Headers_CartHeaderId",
                table: "Details",
                column: "CartHeaderId",
                principalTable: "Headers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Headers_CartHeaderId",
                table: "Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Details",
                table: "Details");

            migrationBuilder.RenameTable(
                name: "Details",
                newName: "Descriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Details_CartHeaderId",
                table: "Descriptions",
                newName: "IX_Descriptions_CartHeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Descriptions",
                table: "Descriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_Headers_CartHeaderId",
                table: "Descriptions",
                column: "CartHeaderId",
                principalTable: "Headers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
