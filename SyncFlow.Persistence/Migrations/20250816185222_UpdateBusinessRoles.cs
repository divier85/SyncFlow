using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBusinessRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessAssignments_Roles_RoleId",
                table: "ProcessAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Businesses_BusinessId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Roles_RolesId",
                table: "RoleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Roles_RoleId",
                table: "TaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_BusinessId",
                table: "Role",
                newName: "IX_Role_BusinessId");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationRoleId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationRoleId",
                table: "TaskAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationRoleId",
                table: "ProcessAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApplicationRoleId",
                table: "Users",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_ApplicationRoleId",
                table: "TaskAssignments",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessAssignments_ApplicationRoleId",
                table: "ProcessAssignments",
                column: "ApplicationRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessAssignments_AspNetRoles_ApplicationRoleId",
                table: "ProcessAssignments",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessAssignments_Role_RoleId",
                table: "ProcessAssignments",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Businesses_BusinessId",
                table: "Role",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Role_RolesId",
                table: "RoleUser",
                column: "RolesId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_AspNetRoles_ApplicationRoleId",
                table: "TaskAssignments",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Role_RoleId",
                table: "TaskAssignments",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetRoles_ApplicationRoleId",
                table: "Users",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessAssignments_AspNetRoles_ApplicationRoleId",
                table: "ProcessAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessAssignments_Role_RoleId",
                table: "ProcessAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Businesses_BusinessId",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Role_RolesId",
                table: "RoleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_AspNetRoles_ApplicationRoleId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Role_RoleId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetRoles_ApplicationRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ApplicationRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignments_ApplicationRoleId",
                table: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ProcessAssignments_ApplicationRoleId",
                table: "ProcessAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "ProcessAssignments");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_Role_BusinessId",
                table: "Roles",
                newName: "IX_Roles_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessAssignments_Roles_RoleId",
                table: "ProcessAssignments",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Businesses_BusinessId",
                table: "Roles",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Roles_RolesId",
                table: "RoleUser",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Roles_RoleId",
                table: "TaskAssignments",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
