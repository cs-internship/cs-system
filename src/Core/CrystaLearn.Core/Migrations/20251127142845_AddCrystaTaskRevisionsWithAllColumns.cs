using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCrystaTaskRevisionsWithAllColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
              name: "CrystaTaskRevisions",
              schema: "CrystaLearn");

            migrationBuilder.CreateTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CrystaTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    RevisionCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProviderTaskId = table.Column<string>(type: "text", nullable: true),
                    AssignedToId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompletedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedToText = table.Column<string>(type: "text", nullable: true),
                    CompletedByText = table.Column<string>(type: "text", nullable: true),
                    CreatedByText = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DescriptionHtml = table.Column<string>(type: "text", nullable: true),
                    TaskCreateDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TaskDoneDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TaskAssignDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    ProviderTaskUrl = table.Column<string>(type: "text", nullable: true),
                    WorkItemSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WorkItemSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WorkItemSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WorkItemSyncInfo_ContentHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WorkItemSyncInfo_SyncGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WorkItemSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    WorkItemSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WorkItemSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    RevisionsSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RevisionsSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisionsSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisionsSyncInfo_ContentHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RevisionsSyncInfo_SyncGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RevisionsSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    RevisionsSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisionsSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    UpdatesSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatesSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatesSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatesSyncInfo_ContentHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatesSyncInfo_SyncGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatesSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    UpdatesSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatesSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CommentsSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommentsSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CommentsSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CommentsSyncInfo_ContentHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommentsSyncInfo_SyncGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommentsSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    CommentsSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CommentsSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CrystaProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    WorkItemType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Reason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AreaPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IterationPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedByDisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedByIdString = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ChangedById = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ChangedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Revision = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProjectName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    IterationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Severity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: true),
                    OriginalEstimate = table.Column<double>(type: "double precision", nullable: true),
                    RemainingWork = table.Column<double>(type: "double precision", nullable: true),
                    CompletedWork = table.Column<double>(type: "double precision", nullable: true),
                    StoryPoints = table.Column<double>(type: "double precision", nullable: true),
                    Tags = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AttachmentsCount = table.Column<int>(type: "integer", nullable: false),
                    Relations = table.Column<string>(type: "text", nullable: true),
                    Links = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CommentCount = table.Column<int>(type: "integer", nullable: false),
                    BoardColumn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BoardColumnDone = table.Column<bool>(type: "boolean", nullable: false),
                    StateChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ResolvedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExternalId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CustomFields = table.Column<string>(type: "text", nullable: true),
                    RawJson = table.Column<string>(type: "text", nullable: true),
                    SystemFields = table.Column<string>(type: "text", nullable: true),
                    CreatedFromRevisionId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrystaTaskRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrystaTaskRevisions_CrystaPrograms_CrystaProgramId",
                        column: x => x.CrystaProgramId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTaskRevisions_CrystaTasks_CrystaTaskId",
                        column: x => x.CrystaTaskId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrystaTaskRevisions_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTaskRevisions_Users_CompletedById",
                        column: x => x.CompletedById,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTaskRevisions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_ParentId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_AssignedToId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "ChangedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_CompletedById",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_CreatedById",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_CrystaProgramId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "CrystaProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "CrystaTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_ProjectId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "ProviderTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_Revision",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "Revision");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_State",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "State");

            migrationBuilder.AddForeignKey(
                name: "FK_CrystaTasks_CrystaTasks_ParentId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "ParentId",
                principalSchema: "CrystaLearn",
                principalTable: "CrystaTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrystaTasks_CrystaTasks_ParentId",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_ParentId",
                schema: "CrystaLearn",
                table: "CrystaTasks");

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
    }
}
