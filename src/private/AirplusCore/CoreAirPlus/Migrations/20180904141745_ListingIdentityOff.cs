using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoreAirPlus.Migrations
{
    public partial class ListingIdentityOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_calendarPrices_Listings_ListingId",
                table: "calendarPrices");


            migrationBuilder.DropPrimaryKey(
                name: "PK_Listings",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Listings");

            //migrationBuilder.AlterColumn<long>(
            //    name: "ListingId",
            //    table: "Listings",
            //    nullable: false,
            //    oldClrType: typeof(long))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "ListingId",
                table: "Listings",
                nullable: false,
                defaultValue: 0);


            //migrationBuilder.AddColumn<int>(
            //    name: "PropertyId",
            //    table: "calendarPrices",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Listings",
                table: "Listings",
                columns: new[] { "ListingId", "PropertyId" });

            migrationBuilder.CreateIndex(
                name: "IX_calendarPrices_ListingId_PropertyId",
                table: "calendarPrices",
                columns: new[] { "ListingId", "PropertyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_calendarPrices_Listings_ListingId_PropertyId",
                table: "calendarPrices",
                columns: new[] { "ListingId", "PropertyId" },
                principalTable: "Listings",
                principalColumns: new[] { "ListingId", "PropertyId" },
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_calendarPrices_Listings_ListingId_PropertyId",
                table: "calendarPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Listings",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_calendarPrices_ListingId_PropertyId",
                table: "calendarPrices");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "calendarPrices");

            migrationBuilder.AlterColumn<long>(
                name: "ListingId",
                table: "Listings",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Listings",
                table: "Listings",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_calendarPrices_Listings_ListingId",
                table: "calendarPrices",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
