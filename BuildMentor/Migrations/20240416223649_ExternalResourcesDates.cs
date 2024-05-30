using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildMentor.Migrations
{
    /// <inheritdoc />
    public partial class ExternalResourcesDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Instructions",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Instructions",
                newName: "Date");
        }
    }
}
