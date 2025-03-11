using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karikari_backend.Migrations
{
    /// <inheritdoc />
    public partial class fixloan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Loans",
                table: "Loans");

            migrationBuilder.RenameTable(
                name: "Loans",
                newName: "Loan");

            migrationBuilder.RenameColumn(
                name: "RepayerID",
                table: "Loan",
                newName: "RepayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loan",
                table: "Loan",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_EventId",
                table: "Loan",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Events_EventId",
                table: "Loan",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Events_EventId",
                table: "Loan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loan",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_EventId",
                table: "Loan");

            migrationBuilder.RenameTable(
                name: "Loan",
                newName: "Loans");

            migrationBuilder.RenameColumn(
                name: "RepayerId",
                table: "Loans",
                newName: "RepayerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loans",
                table: "Loans",
                column: "Id");
        }
    }
}
