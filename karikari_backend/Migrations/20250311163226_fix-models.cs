using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karikari_backend.Migrations
{
    /// <inheritdoc />
    public partial class fixmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Events_EventId",
                table: "Loan");

            migrationBuilder.RenameColumn(
                name: "RepayerId",
                table: "Loan",
                newName: "Repayer");

            migrationBuilder.RenameColumn(
                name: "PayerId",
                table: "Loan",
                newName: "Payer");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Events",
                newName: "Group");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Loan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Event",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Events_EventId",
                table: "Loan",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Events_EventId",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "Event",
                table: "Loan");

            migrationBuilder.RenameColumn(
                name: "Repayer",
                table: "Loan",
                newName: "RepayerId");

            migrationBuilder.RenameColumn(
                name: "Payer",
                table: "Loan",
                newName: "PayerId");

            migrationBuilder.RenameColumn(
                name: "Group",
                table: "Events",
                newName: "GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Events_EventId",
                table: "Loan",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
