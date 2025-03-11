using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karikari_backend.Migrations
{
    /// <inheritdoc />
    public partial class fixloan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_PayerId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_RepayerId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_EventId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_PayerId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_RepayerId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "RepayerId",
                table: "Loans",
                newName: "RepayerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RepayerID",
                table: "Loans",
                newName: "RepayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_EventId",
                table: "Loans",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_PayerId",
                table: "Loans",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_RepayerId",
                table: "Loans",
                column: "RepayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_PayerId",
                table: "Loans",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_RepayerId",
                table: "Loans",
                column: "RepayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
