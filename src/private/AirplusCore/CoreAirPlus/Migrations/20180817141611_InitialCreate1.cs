using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoreAirPlus.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_ccompanies_CCId",
                table: "reservations");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_ccompanies_CCId",
                table: "reservations",
                column: "CCId",
                principalTable: "ccompanies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_ccompanies_CCId",
                table: "reservations");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_ccompanies_CCId",
                table: "reservations",
                column: "CCId",
                principalTable: "ccompanies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
