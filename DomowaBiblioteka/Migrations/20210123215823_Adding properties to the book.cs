using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DomowaBiblioteka.Migrations
{
    public partial class Addingpropertiestothebook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Borrower",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfRent",
                table: "Books",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RentalApprovingPerson",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Borrower",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DateOfRent",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RentalApprovingPerson",
                table: "Books");
        }
    }
}
