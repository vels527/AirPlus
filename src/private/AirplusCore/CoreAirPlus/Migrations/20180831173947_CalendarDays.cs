using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoreAirPlus.Migrations
{
    public partial class CalendarDays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calendarPrices",
                columns: table => new
                {
                    ListingID = table.Column<long>(nullable: false),
                    CalendarDate = table.Column<DateTime>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PropertyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendarPrices", x => new { x.ListingID, x.CalendarDate });
                    table.UniqueConstraint("AK_calendarPrices_CalendarDate_ListingID", x => new { x.CalendarDate, x.ListingID });
                    table.ForeignKey(
                        name: "FK_calendarPrices_properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "properties",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_calendarPrices_PropertyId",
                table: "calendarPrices",
                column: "PropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calendarPrices");
        }
    }
}
