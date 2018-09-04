using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoreAirPlus.Migrations
{
    public partial class MultipleListings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_calendarPrices_CalendarDetailListingId_CalendarDetailCalendarDate",
                table: "Listings");

            //migrationBuilder.DropIndex(
            //    name: "IX_Listings_CalendarDetailListingId_CalendarDetailCalendarDate",
            //    table: "Listings");

            migrationBuilder.DropColumn(
                name: "CalendarDetailCalendarDate",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CalendarDetailListingId",
                table: "Listings");

            migrationBuilder.AddForeignKey(
                name: "FK_calendarPrices_Listings_ListingId",
                table: "calendarPrices",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_calendarPrices_Listings_ListingId",
                table: "calendarPrices");

            migrationBuilder.AddColumn<DateTime>(
                name: "CalendarDetailCalendarDate",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CalendarDetailListingId",
                table: "Listings",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Listings_CalendarDetailListingId_CalendarDetailCalendarDate",
            //    table: "Listings",
            //    columns: new[] { "CalendarDetailListingId", "CalendarDetailCalendarDate" },
            //    unique: true,
            //    filter: "[CalendarDetailListingId] IS NOT NULL AND [CalendarDetailCalendarDate] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_calendarPrices_CalendarDetailListingId_CalendarDetailCalendarDate",
                table: "Listings",
                columns: new[] { "CalendarDetailListingId", "CalendarDetailCalendarDate" },
                principalTable: "calendarPrices",
                principalColumns: new[] { "ListingId", "CalendarDate" },
                onDelete: ReferentialAction.SetNull);
        }
    }
}
