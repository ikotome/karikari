using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shogendar.Karikari.Backend.Migrations
{
    /// <inheritdoc />
    public partial class fixloanmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Event",
                table: "Loans");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Loans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Event",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
