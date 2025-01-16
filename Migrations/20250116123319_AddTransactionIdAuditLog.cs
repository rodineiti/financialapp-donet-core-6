using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAppMvc.Migrations
{
    public partial class AddTransactionIdAuditLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "AuditLogs",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "AuditLogs");
        }
    }
}
