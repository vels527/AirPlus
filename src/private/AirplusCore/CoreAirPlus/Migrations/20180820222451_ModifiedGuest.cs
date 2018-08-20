using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoreAirPlus.Migrations
{
    public partial class ModifiedGuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "hosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "hosts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "hosts",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<byte>(
                name: "Age",
                table: "hosts",
                nullable: true,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "guests",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<byte>(
                name: "Age",
                table: "guests",
                nullable: true,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "hosts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "hosts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "hosts",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Age",
                table: "hosts",
                nullable: false,
                oldClrType: typeof(byte),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "guests",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Age",
                table: "guests",
                nullable: false,
                oldClrType: typeof(byte),
                oldNullable: true);
        }
    }
}
