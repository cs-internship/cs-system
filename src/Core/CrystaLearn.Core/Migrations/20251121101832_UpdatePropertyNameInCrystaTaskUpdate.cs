using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertyNameInCrystaTaskUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskUpdates_AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates");

            migrationBuilder.DropColumn(
                name: "AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates");

            migrationBuilder.RenameColumn(
                name: "AzureUpdateId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                newName: "ProviderUpdateId");

            migrationBuilder.AlterColumn<string>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "ProviderTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskUpdates_ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates");

            migrationBuilder.DropColumn(
                name: "ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates");

            migrationBuilder.RenameColumn(
                name: "ProviderUpdateId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                newName: "AzureUpdateId");

            migrationBuilder.AlterColumn<int>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "AzureWorkItemId");
        }
    }
}
