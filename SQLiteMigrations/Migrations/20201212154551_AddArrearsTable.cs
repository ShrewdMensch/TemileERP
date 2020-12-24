using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLiteMigrations.Migrations
{
    public partial class AddArrearsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arrears",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    AffectedPayrollId = table.Column<string>(nullable: true),
                    CorrectivePayrollId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arrears_Payrolls_AffectedPayrollId",
                        column: x => x.AffectedPayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Arrears_Payrolls_CorrectivePayrollId",
                        column: x => x.CorrectivePayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arrears_AffectedPayrollId",
                table: "Arrears",
                column: "AffectedPayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrears_CorrectivePayrollId",
                table: "Arrears",
                column: "CorrectivePayrollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arrears");
        }
    }
}
