using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLiteMigrations.Migrations
{
    public partial class EmailSentToBankLogsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailSentToBankLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    AddedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    Vessel = table.Column<string>(nullable: true),
                    SentCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSentToBankLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailSentToBankLogs_AspNetUsers_AddedById",
                        column: x => x.AddedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailSentToBankLogs_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailSentToBankLogs_AddedById",
                table: "EmailSentToBankLogs",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSentToBankLogs_ModifiedById",
                table: "EmailSentToBankLogs",
                column: "ModifiedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailSentToBankLogs");
        }
    }
}
