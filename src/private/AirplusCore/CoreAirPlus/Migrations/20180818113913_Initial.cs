using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoreAirPlus.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ccompanies",
                columns: table => new
                {
                    CleaningCompanyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ccompanies", x => x.CleaningCompanyId);
                });

            migrationBuilder.CreateTable(
                name: "guests",
                columns: table => new
                {
                    GuestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Age = table.Column<byte>(nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guests", x => x.GuestId);
                });

            migrationBuilder.CreateTable(
                name: "hosts",
                columns: table => new
                {
                    HostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Age = table.Column<byte>(nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hosts", x => x.HostId);
                });

            migrationBuilder.CreateTable(
                name: "properties",
                columns: table => new
                {
                    PropertyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: false),
                    HostId = table.Column<int>(nullable: false),
                    ICALURL = table.Column<string>(nullable: true),
                    ListingId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_properties", x => x.PropertyId);
                    table.ForeignKey(
                        name: "FK_properties_hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "hosts",
                        principalColumn: "HostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    GuestId = table.Column<int>(nullable: false),
                    PropertyId = table.Column<int>(nullable: false),
                    CheckIn = table.Column<DateTime>(nullable: false),
                    CheckOut = table.Column<DateTime>(nullable: false),
                    CleaningCompanyId = table.Column<int>(nullable: false),
                    CleaningTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    RCheckIn = table.Column<DateTime>(nullable: true),
                    RCheckOut = table.Column<DateTime>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => new { x.GuestId, x.PropertyId, x.CheckIn, x.CheckOut });
                    table.UniqueConstraint("AK_reservations_CheckIn_CheckOut_GuestId_PropertyId", x => new { x.CheckIn, x.CheckOut, x.GuestId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_reservations_ccompanies_CleaningCompanyId",
                        column: x => x.CleaningCompanyId,
                        principalTable: "ccompanies",
                        principalColumn: "CleaningCompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "guests",
                        principalColumn: "GuestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "properties",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_properties_HostId",
                table: "properties",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_CleaningCompanyId",
                table: "reservations",
                column: "CleaningCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_PropertyId",
                table: "reservations",
                column: "PropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "ccompanies");

            migrationBuilder.DropTable(
                name: "guests");

            migrationBuilder.DropTable(
                name: "properties");

            migrationBuilder.DropTable(
                name: "hosts");
        }
    }
}
