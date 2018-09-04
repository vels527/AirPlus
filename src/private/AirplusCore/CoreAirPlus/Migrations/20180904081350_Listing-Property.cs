using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CoreAirPlus.Migrations
{
    public partial class ListingProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Listings",
               columns: table => new
               {
                   ListingId = table.Column<long>(nullable: false)
                       .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                   CalendarDetailCalendarDate = table.Column<DateTime>(nullable: true),
                   CalendarDetailListingID = table.Column<long>(nullable: true),
                   PropertyId = table.Column<int>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Listings", x => x.ListingId);
                   table.ForeignKey(
                       name: "FK_Listings_properties_PropertyId",
                       column: x => x.PropertyId,
                       principalTable: "properties",
                       principalColumn: "PropertyId",
                       onDelete: ReferentialAction.Cascade);
                   table.ForeignKey(
                       name: "FK_Listings_calendarPrices_CalendarDetailListingID_CalendarDetailCalendarDate",
                       columns: x => new { x.CalendarDetailListingID, x.CalendarDetailCalendarDate },
                       principalTable: "calendarPrices",
                       principalColumns: new[] { "ListingID", "CalendarDate" },
                       onDelete: ReferentialAction.SetNull);
               });
            migrationBuilder.CreateIndex(
    name: "IX_Listings_PropertyId",
    table: "Listings",
    column: "PropertyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Listings_CalendarDetailListingID_CalendarDetailCalendarDate",
            //    table: "Listings",
            //    columns: new[] { "CalendarDetailListingID", "CalendarDetailCalendarDate" },
            //    unique: true,
            //    filter: "[CalendarDetailListingID] IS NOT NULL AND [CalendarDetailCalendarDate] IS NOT NULL");
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Listings_calendarPrices_CalendarDetailListingID_CalendarDetailCalendarDate",
            //    table: "Listings");

            //migrationBuilder.DropIndex(
            //    name: "IX_Listings_CalendarDetailListingID_CalendarDetailCalendarDate",
            //    table: "Listings");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_calendarPrices_CalendarDate_ListingID",
            //    table: "calendarPrices");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_calendarPrices",
            //    table: "calendarPrices");


            //migrationBuilder.RenameColumn(
            //    name: "CalendarDetailListingID",
            //    table: "Listings",
            //    newName: "CalendarDetailListingId");


            //migrationBuilder.CreateIndex(
            //    name: "IX_Listings_CalendarDetailListingId_CalendarDetailCalendarDate",
            //    table: "Listings",
            //    columns: new[] { "CalendarDetailListingId", "CalendarDetailCalendarDate" },
            //    unique: true,
            //    filter: "[CalendarDetailListingId] IS NOT NULL AND [CalendarDetailCalendarDate] IS NOT NULL");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Listings_calendarPrices_CalendarDetailListingId_CalendarDetailCalendarDate",
            //    table: "Listings",
            //    columns: new[] { "CalendarDetailListingId", "CalendarDetailCalendarDate" },
            //    principalTable: "calendarPrices",
            //    principalColumns: new[] { "ListingId", "CalendarDate" },
            //    onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_calendarPrices_CalendarDetailListingId_CalendarDetailCalendarDate",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CalendarDetailListingId_CalendarDetailCalendarDate",
                table: "Listings");



            migrationBuilder.RenameColumn(
                name: "CalendarDetailListingId",
                table: "Listings",
                newName: "CalendarDetailListingID");

            migrationBuilder.AddColumn<long>(
                name: "ListingID",
                table: "calendarPrices",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_calendarPrices_CalendarDate_ListingID",
                table: "calendarPrices",
                columns: new[] { "CalendarDate", "ListingID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_calendarPrices",
                table: "calendarPrices",
                columns: new[] { "ListingID", "CalendarDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CalendarDetailListingID_CalendarDetailCalendarDate",
                table: "Listings",
                columns: new[] { "CalendarDetailListingID", "CalendarDetailCalendarDate" },
                unique: true,
                filter: "[CalendarDetailListingID] IS NOT NULL AND [CalendarDetailCalendarDate] IS NOT NULL");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Listings_calendarPrices_CalendarDetailListingID_CalendarDetailCalendarDate",
            //    table: "Listings",
            //    columns: new[] { "CalendarDetailListingID", "CalendarDetailCalendarDate" },
            //    principalTable: "calendarPrices",
            //    principalColumns: new[] { "ListingID", "CalendarDate" },
            //    onDelete: ReferentialAction.SetNull);
        }
    }
}
