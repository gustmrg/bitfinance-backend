using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitFinance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoneyVOAndTableIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_due",
                table: "bills");

            migrationBuilder.AlterColumn<string>(
                name: "timezone_id",
                table: "organizations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "America/Sao_Paulo",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<bool>(
                name: "is_used",
                table: "organization_invites",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "organization_invites",
                type: "timestamp(3) with time zone",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "organization_invites",
                type: "timestamp(3) with time zone",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "amount_currency",
                table: "expenses",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "BRL");

            migrationBuilder.AddColumn<string>(
                name: "amount_paid_currency",
                table: "bills",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "uploaded_at",
                table: "bill_documents",
                type: "timestamp(3) with time zone",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "storage_provider",
                table: "bill_documents",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "file_hash",
                table: "bill_documents",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "document_type",
                table: "bill_documents",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "deleted_at",
                table: "bill_documents",
                type: "timestamp(3) with time zone",
                precision: 3,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content_type",
                table: "bill_documents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "money",
                columns: table => new
                {
                    BillId = table.Column<Guid>(type: "uuid", nullable: false),
                    amount_due = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    amount_due_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_money", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_money_bills_BillId",
                        column: x => x.BillId,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_organization_invites_organization_id",
                table: "organization_invites",
                column: "organization_id");

            migrationBuilder.AddForeignKey(
                name: "FK_organization_invites_organizations_organization_id",
                table: "organization_invites",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_organization_invites_organizations_organization_id",
                table: "organization_invites");

            migrationBuilder.DropTable(
                name: "money");

            migrationBuilder.DropIndex(
                name: "IX_organization_invites_organization_id",
                table: "organization_invites");

            migrationBuilder.DropColumn(
                name: "amount_currency",
                table: "expenses");

            migrationBuilder.DropColumn(
                name: "amount_paid_currency",
                table: "bills");

            migrationBuilder.AlterColumn<string>(
                name: "timezone_id",
                table: "organizations",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldDefaultValue: "America/Sao_Paulo");

            migrationBuilder.AlterColumn<bool>(
                name: "is_used",
                table: "organization_invites",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "organization_invites",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "organization_invites",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldPrecision: 3);

            migrationBuilder.AddColumn<decimal>(
                name: "amount_due",
                table: "bills",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "uploaded_at",
                table: "bill_documents",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<int>(
                name: "storage_provider",
                table: "bill_documents",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "file_hash",
                table: "bill_documents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "document_type",
                table: "bill_documents",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "deleted_at",
                table: "bill_documents",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) with time zone",
                oldPrecision: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content_type",
                table: "bill_documents",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
