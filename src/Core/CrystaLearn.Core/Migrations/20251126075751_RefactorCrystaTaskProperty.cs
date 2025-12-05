using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCrystaTaskProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskComments_AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropColumn(
                name: "AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.RenameColumn(
                name: "AzureCommentId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "ProviderCommentId");

            migrationBuilder.AlterColumn<string>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RevisionCode",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "CrystaTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_Revision",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "Revision");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "ProviderTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrystaTasks_CrystaTasks_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "CrystaTaskId",
                principalSchema: "CrystaLearn",
                principalTable: "CrystaTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrystaTasks_CrystaTasks_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_Revision",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskComments_ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropColumn(
                name: "CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "RevisionCode",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.RenameColumn(
                name: "ProviderCommentId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "AzureCommentId");

            migrationBuilder.AlterColumn<int>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CrystaTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttachmentsSnapshot = table.Column<string>(type: "text", nullable: true),
                    AzureWorkItemId = table.Column<int>(type: "integer", nullable: false),
                    ChangeDetails = table.Column<string>(type: "text", nullable: true),
                    ChangeSummary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ChangedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ChangedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CommentsSnapshot = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedFromUpdateId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FieldsSnapshot = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RawJson = table.Column<string>(type: "text", nullable: true),
                    RelationsSnapshot = table.Column<string>(type: "text", nullable: true),
                    RevisionCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RevisionNumber = table.Column<int>(type: "integer", nullable: false),
                    SnapshotJson = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrystaTaskRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrystaTaskRevisions_CrystaTasks_CrystaTaskId",
                        column: x => x.CrystaTaskId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "AzureWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "AzureWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "ChangedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "CrystaTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_RevisionNumber",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "RevisionNumber");
        }
    }
}
