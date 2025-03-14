using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shogendar.Karikari.Backend.Migrations
{
    /// <inheritdoc />
    public partial class deleteeventmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Loans_EventId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Loans",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Method",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RepayDate",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "RepayDate",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Loans",
                newName: "EventId");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Group = table.Column<int>(type: "int", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Settled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_EventId",
                table: "Loans",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Events_EventId",
                table: "Loans",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
