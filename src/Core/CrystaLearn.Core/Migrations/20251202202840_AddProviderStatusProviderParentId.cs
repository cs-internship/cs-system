using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderStatusProviderParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderParentId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderStatus",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderParentId",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "ProviderStatus",
                schema: "CrystaLearn",
                table: "CrystaTasks");
        }
    }
}
