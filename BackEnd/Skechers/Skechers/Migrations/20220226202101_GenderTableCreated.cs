﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Skechers.Migrations
{
    public partial class GenderTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Product",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_GenderId",
                table: "Product",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Genders_GenderId",
                table: "Product",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Genders_GenderId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropIndex(
                name: "IX_Product_GenderId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Product");
        }
    }
}
