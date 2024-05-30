using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildMentor.Migrations
{
    /// <inheritdoc />
    public partial class comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalResource_Instructions_InstructionId",
                table: "ExternalResource");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolPermissionRequests_AspNetUsers_AdminId",
                table: "ToolPermissionRequests");

            migrationBuilder.DropIndex(
                name: "IX_ToolPermissionRequests_AdminId",
                table: "ToolPermissionRequests");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "ToolPermissionRequests");

            migrationBuilder.RenameColumn(
                name: "Commment",
                table: "ToolPermissionRequests",
                newName: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "InstructionId",
                table: "ExternalResource",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalResource_Instructions_InstructionId",
                table: "ExternalResource",
                column: "InstructionId",
                principalTable: "Instructions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalResource_Instructions_InstructionId",
                table: "ExternalResource");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "ToolPermissionRequests",
                newName: "Commment");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "ToolPermissionRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "InstructionId",
                table: "ExternalResource",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolPermissionRequests_AdminId",
                table: "ToolPermissionRequests",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalResource_Instructions_InstructionId",
                table: "ExternalResource",
                column: "InstructionId",
                principalTable: "Instructions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolPermissionRequests_AspNetUsers_AdminId",
                table: "ToolPermissionRequests",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
