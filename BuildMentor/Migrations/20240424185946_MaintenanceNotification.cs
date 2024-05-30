using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildMentor.Migrations
{
    /// <inheritdoc />
    public partial class MaintenanceNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_SenderId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ToolMaintenanceRecords_ToolMaintenanceId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserTools_UserToolId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ToolMaintenanceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "ToolMaintenanceId",
                table: "Notifications",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "UserToolId",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Notifications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ActiveMaintenances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserToolId = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveMaintenances_UserTools_UserToolId",
                        column: x => x.UserToolId,
                        principalTable: "UserTools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveMaintenances_UserToolId",
                table: "ActiveMaintenances",
                column: "UserToolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserTools_UserToolId",
                table: "Notifications",
                column: "UserToolId",
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

            migrationBuilder.DropTable(
                name: "ActiveMaintenances");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Notifications",
                newName: "ToolMaintenanceId");

            migrationBuilder.AlterColumn<int>(
                name: "UserToolId",
                table: "Notifications",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ToolMaintenanceId",
                table: "Notifications",
                column: "ToolMaintenanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_SenderId",
                table: "Notifications",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ToolMaintenanceRecords_ToolMaintenanceId",
                table: "Notifications",
                column: "ToolMaintenanceId",
                principalTable: "ToolMaintenanceRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserTools_UserToolId",
                table: "Notifications",
                column: "UserToolId",
                principalTable: "UserTools",
                principalColumn: "Id");
        }
    }
}
