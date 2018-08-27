using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoreAirPlus.Migrations
{
    public partial class ini20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "hosts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "hosts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "hosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ReservationViewModel",
                columns: table => new
                {
                    GuestId = table.Column<int>(nullable: false),
                    PropertyId = table.Column<int>(nullable: false),
                    CheckIn = table.Column<DateTime>(nullable: false),
                    CheckOut = table.Column<DateTime>(nullable: false),
                    CleaningCompanyId = table.Column<int>(nullable: true),
                    CleaningTime = table.Column<DateTime>(nullable: true),
                    GuestName = table.Column<string>(nullable: true),
                    RCheckIn = table.Column<string>(nullable: true),
                    RCheckOut = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationViewModel", x => new { x.GuestId, x.PropertyId, x.CheckIn, x.CheckOut });
                    table.UniqueConstraint("AK_ReservationViewModel_CheckIn_GuestId_PropertyId", x => new { x.CheckIn, x.GuestId, x.PropertyId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationViewModel");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "hosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "hosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "hosts",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
