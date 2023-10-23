using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystallineSociety.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class DeletePrerequisites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prerequisites",
                table: "Badges");

            migrationBuilder.RenameColumn(
                name: "PrerequisitesJsonSourceUrl",
                table: "Badges",
                newName: "SpecJsonSourceUrl");

            migrationBuilder.RenameColumn(
                name: "PrerequisitesJson",
                table: "Badges",
                newName: "SpecJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecJsonSourceUrl",
                table: "Badges",
                newName: "PrerequisitesJsonSourceUrl");

            migrationBuilder.RenameColumn(
                name: "SpecJson",
                table: "Badges",
                newName: "PrerequisitesJson");

            migrationBuilder.AddColumn<string>(
                name: "Prerequisites",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
