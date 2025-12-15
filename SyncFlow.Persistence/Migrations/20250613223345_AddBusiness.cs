using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_BusinessId",
                table: "Users",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_BusinessId",
                table: "Roles",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BusinessId",
                table: "Projects",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Business_BusinessId",
                table: "Projects",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Business_BusinessId",
                table: "Roles",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Business_BusinessId",
                table: "Users",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Business_BusinessId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Business_BusinessId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Business_BusinessId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropIndex(
                name: "IX_Users_BusinessId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Roles_BusinessId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Projects_BusinessId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Projects");
        }
    }
}
