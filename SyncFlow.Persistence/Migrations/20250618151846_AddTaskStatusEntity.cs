using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskStatusEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TaskStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Core = table.Column<byte>(type: "tinyint", nullable: false),
                    UIColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskStatuses_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"
                    DECLARE @bizId UNIQUEIDENTIFIER
                    DECLARE cur CURSOR FOR SELECT Id FROM Businesses
                    OPEN cur
                    FETCH NEXT FROM cur INTO @bizId
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        INSERT INTO TaskStatuses (Id, Name, Core, BusinessId, CreatedAt)
                        VALUES (NEWID(), 'Pending', 0, @bizId, GETDATE())
                        FETCH NEXT FROM cur INTO @bizId
                    END
                    CLOSE cur
                    DEALLOCATE cur
                    ");

            // 2. Actualizar Tasks con NULL o 0000... al Pending del mismo negocio
            migrationBuilder.Sql(@"
                    UPDATE t SET StatusId = s.Id
                    FROM Tasks t
                    JOIN TaskStatuses s ON s.BusinessId = t.BusinessId AND s.Name = 'Pending'
                    WHERE t.StatusId IS NULL OR t.StatusId = '00000000-0000-0000-0000-000000000000'
                    ");


            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskStatuses_BusinessId_Name",
                table: "TaskStatuses",
                columns: new[] { "BusinessId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "TaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Tasks");
        }
    }
}
