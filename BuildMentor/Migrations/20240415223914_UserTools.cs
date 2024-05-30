using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildMentor.Migrations
{
    /// <inheritdoc />
    public partial class UserTools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Tools_ToolId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolMaintenanceRecords_Tools_ToolId",
                table: "ToolMaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Tools");

            migrationBuilder.DropColumn(
                name: "WarrantyExpirationDate",
                table: "Tools");

            migrationBuilder.RenameColumn(
                name: "ToolId",
                table: "Notifications",
                newName: "UserToolId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ToolId",
                table: "Notifications",
                newName: "IX_Notifications_UserToolId");

            migrationBuilder.CreateTable(
                name: "UserTools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WarrantyExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Condition = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTools_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTools_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTools_ToolId",
                table: "UserTools",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTools_UserId",
                table: "UserTools",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserTools_UserToolId",
                table: "Notifications",
                column: "UserToolId",
                principalTable: "UserTools",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMaintenanceRecords_UserTools_ToolId",
                table: "ToolMaintenanceRecords",
                column: "ToolId",
                principalTable: "UserTools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserTools_UserToolId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolMaintenanceRecords_UserTools_ToolId",
                table: "ToolMaintenanceRecords");

            migrationBuilder.DropTable(
                name: "UserTools");

            migrationBuilder.RenameColumn(
                name: "UserToolId",
                table: "Notifications",
                newName: "ToolId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserToolId",
                table: "Notifications",
                newName: "IX_Notifications_ToolId");

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "Tools",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyExpirationDate",
                table: "Tools",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Tools_ToolId",
                table: "Notifications",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMaintenanceRecords_Tools_ToolId",
                table: "ToolMaintenanceRecords",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
