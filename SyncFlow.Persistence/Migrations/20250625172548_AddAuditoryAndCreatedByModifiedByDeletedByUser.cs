using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditoryAndCreatedByModifiedByDeletedByUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "TaskTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "TaskTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "TaskTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "TaskStatuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "TaskStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "TaskStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "TaskFeedbacks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "TaskFeedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "TaskFeedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "TaskAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "TaskAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "TaskAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "ProcessTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "ProcessTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProcessTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "ProcessTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "ProcessTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "ProcessTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "ProcessAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "ProcessAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "ProcessAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "PhaseTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "PhaseTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "PhaseTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Phases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Phases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Phases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditLogEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogEntries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogEntries");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TaskTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "TaskTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TaskStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "TaskStatuses");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TaskFeedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "TaskFeedbacks");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskFeedbacks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProcessAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "ProcessAssignments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ProcessAssignments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "PhaseTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "PhaseTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "PhaseTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Businesses");
        }
    }
}
