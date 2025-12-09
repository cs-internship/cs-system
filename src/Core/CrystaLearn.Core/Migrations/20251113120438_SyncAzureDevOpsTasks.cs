using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class SyncAzureDevOpsTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrystaPrograms",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BadgeSystemUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DocumentUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DocumentSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DocumentSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DocumentSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DocumentSyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DocumentSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    DocumentSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DocumentSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    BadgeSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BadgeSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    BadgeSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    BadgeSyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BadgeSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    BadgeSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    BadgeSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrystaPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrystaTasks",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ProviderTaskId = table.Column<string>(type: "text", nullable: true),
                    AssignedToId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompletedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedById1 = table.Column<Guid>(type: "uuid", nullable: true),
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
                    WorkItemSyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WorkItemSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    WorkItemSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WorkItemSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    RevisionsSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RevisionsSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisionsSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisionsSyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RevisionsSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    RevisionsSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisionsSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    UpdatesSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatesSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatesSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatesSyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatesSyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    UpdatesSyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatesSyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CommentsSyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommentsSyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CommentsSyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CommentsSyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    CreatedById = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ChangedById = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ChangedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevisedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Revision = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_CrystaTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrystaTasks_CrystaPrograms_CrystaProgramId",
                        column: x => x.CrystaProgramId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTasks_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTasks_Users_CompletedById",
                        column: x => x.CompletedById,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTasks_Users_CreatedById1",
                        column: x => x.CreatedById1,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CrystaTaskComments",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AzureWorkItemId = table.Column<int>(type: "integer", nullable: false),
                    AzureCommentId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ThreadId = table.Column<int>(type: "integer", nullable: true),
                    ParentCommentId = table.Column<int>(type: "integer", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    FormattedText = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedById = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EditedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EditedById = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EditedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    CommentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Visibility = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RawJson = table.Column<string>(type: "text", nullable: true),
                    ProviderCommentUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Reactions = table.Column<string>(type: "text", nullable: true),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    CrystaTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    SyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    SyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CrystaProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    ContentHtml = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrystaTaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrystaTaskComments_CrystaPrograms_CrystaProgramId",
                        column: x => x.CrystaProgramId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTaskComments_CrystaTasks_CrystaTaskId",
                        column: x => x.CrystaTaskId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrystaTaskComments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AzureWorkItemId = table.Column<int>(type: "integer", nullable: false),
                    RevisionNumber = table.Column<int>(type: "integer", nullable: false),
                    SnapshotJson = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ChangedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ChangedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FieldsSnapshot = table.Column<string>(type: "text", nullable: true),
                    RelationsSnapshot = table.Column<string>(type: "text", nullable: true),
                    AttachmentsSnapshot = table.Column<string>(type: "text", nullable: true),
                    CommentsSnapshot = table.Column<string>(type: "text", nullable: true),
                    ChangeSummary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ChangeDetails = table.Column<string>(type: "text", nullable: true),
                    CreatedFromUpdateId = table.Column<int>(type: "integer", nullable: true),
                    RawJson = table.Column<string>(type: "text", nullable: true),
                    CrystaTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    RevisionCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "CrystaTaskUpdates",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AzureWorkItemId = table.Column<int>(type: "integer", nullable: false),
                    AzureUpdateId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Revision = table.Column<int>(type: "integer", nullable: false),
                    ChangedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ChangedById = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ChangedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FieldName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FieldDisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    Operation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommentText = table.Column<string>(type: "text", nullable: true),
                    IsWorkItemFieldChange = table.Column<bool>(type: "boolean", nullable: false),
                    ChangedPropertiesJson = table.Column<string>(type: "text", nullable: true),
                    RelationChange = table.Column<string>(type: "text", nullable: true),
                    AttachmentChange = table.Column<string>(type: "text", nullable: true),
                    ProviderUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RawJson = table.Column<string>(type: "text", nullable: true),
                    CrystaTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    SyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_SyncHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    SyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CrystaProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrystaTaskUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrystaTaskUpdates_CrystaPrograms_CrystaProgramId",
                        column: x => x.CrystaProgramId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrystaTaskUpdates_CrystaTasks_CrystaTaskId",
                        column: x => x.CrystaTaskId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrystaTaskUpdates_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CrystaLearn",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrystaPrograms_Code",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaPrograms_IsActive",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "AzureWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_CrystaProgramId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "CrystaProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "CrystaTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_UserId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_AssignedToId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "ChangedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_CompletedById",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_CreatedById1",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "CreatedById1");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_CrystaProgramId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "CrystaProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_ProjectId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_ProviderTaskId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "ProviderTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_State",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_AzureWorkItemId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "AzureWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "ChangedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_CrystaProgramId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "CrystaProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "CrystaTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_Revision",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "Revision");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_UserId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrystaTaskComments",
                schema: "CrystaLearn");

            migrationBuilder.DropTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn");

            migrationBuilder.DropTable(
                name: "CrystaTaskUpdates",
                schema: "CrystaLearn");

            migrationBuilder.DropTable(
                name: "CrystaTasks",
                schema: "CrystaLearn");

            migrationBuilder.DropTable(
                name: "CrystaPrograms",
                schema: "CrystaLearn");
        }
    }
}
