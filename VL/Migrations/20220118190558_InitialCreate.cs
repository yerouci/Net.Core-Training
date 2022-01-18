using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    EditorialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pages = table.Column<int>(type: "int", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qualification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.Title);
                    table.ForeignKey(
                        name: "FK_book_author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opinion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "author",
                columns: new[] { "Id", "DateOfBirth", "Name", "Nationality" },
                values: new object[] { 1, new DateTime(1899, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ernest Hemingway", "Estadounidense" });

            migrationBuilder.InsertData(
                table: "author",
                columns: new[] { "Id", "DateOfBirth", "Name", "Nationality" },
                values: new object[] { 2, new DateTime(1809, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edgar Allan Poe", "Estadounidense" });

            migrationBuilder.CreateIndex(
                name: "IX_book_AuthorId",
                table: "book",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_review_UserId",
                table: "review",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
