using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VL.Migrations
{
    public partial class VLDB : Migration
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
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    EditorialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pages = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qualification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_book_author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorUser",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorUser", x => new { x.AuthorsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AuthorUser_author_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorUser_user_UsersId",
                        column: x => x.UsersId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Qualification = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_book_BookId",
                        column: x => x.BookId,
                        principalTable: "book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                values: new object[,]
                {
                    { 1, new DateTime(1899, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ernest Hemingway", "Estadounidense" },
                    { 2, new DateTime(1809, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edgar Allan Poe", "Estadounidense" }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "CreatedAt", "Email", "ImageURL", "Name" },
                values: new object[,]
                {
                    { new Guid("ad3a09d1-9e45-4256-a3a1-9df71226375d"), new DateTime(2022, 1, 20, 14, 55, 36, 596, DateTimeKind.Utc).AddTicks(7391), "user1@gmail.com", "http://user1Image.com/image.jpg", "User1" },
                    { new Guid("e8a7d9e8-bbb3-44f2-b968-368cbbfc23c0"), new DateTime(2022, 1, 20, 14, 55, 36, 596, DateTimeKind.Utc).AddTicks(7940), "user2@gmail.com", null, "User2" },
                    { new Guid("ded8654d-628f-4302-9b21-bba8b1f32186"), new DateTime(2022, 1, 20, 14, 55, 36, 596, DateTimeKind.Utc).AddTicks(7948), "user3@gmail.com", "http://user3Image.com/image.jpg", "User3" },
                    { new Guid("4125cf02-a9b1-428c-a388-4a40fbf0def6"), new DateTime(2022, 1, 20, 14, 55, 36, 596, DateTimeKind.Utc).AddTicks(7954), "user4@gmail.com", "http://user4Image.com/image.jpg", "User4" },
                    { new Guid("86402e28-f9d9-4e76-8cb2-1a8fbc390e45"), new DateTime(2022, 1, 20, 14, 55, 36, 596, DateTimeKind.Utc).AddTicks(7965), "user5@gmail.com", null, "User5" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorUser_UsersId",
                table: "AuthorUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_book_AuthorId",
                table: "book",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_review_BookId",
                table: "review",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_review_UserId",
                table: "review",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorUser");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "author");
        }
    }
}
