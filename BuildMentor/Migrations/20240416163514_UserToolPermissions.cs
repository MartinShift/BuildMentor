using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildMentor.Migrations
{
    /// <inheritdoc />
    public partial class UserToolPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "UserTools",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTools_PermissionId",
                table: "UserTools",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTools_ToolPermissions_PermissionId",
                table: "UserTools",
                column: "PermissionId",
                principalTable: "ToolPermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTools_ToolPermissions_PermissionId",
                table: "UserTools");

            migrationBuilder.DropIndex(
                name: "IX_UserTools_PermissionId",
                table: "UserTools");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "UserTools");
        }
    }
}
