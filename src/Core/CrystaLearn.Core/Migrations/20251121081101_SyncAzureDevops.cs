using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class SyncAzureDevops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadgeSyncInfo_LastSyncDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "BadgeSyncInfo_LastSyncOffset",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "BadgeSyncInfo_SyncEndDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "BadgeSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "BadgeSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "BadgeSyncInfo_SyncStartDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "BadgeSyncInfo_SyncStatus",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "DocumentSyncInfo_LastSyncDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "DocumentSyncInfo_LastSyncOffset",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "DocumentSyncInfo_SyncEndDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "DocumentSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "DocumentSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "DocumentSyncInfo_SyncStartDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.DropColumn(
                name: "DocumentSyncInfo_SyncStatus",
                schema: "CrystaLearn",
                table: "CrystaPrograms");

            migrationBuilder.RenameColumn(
                name: "SyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                newName: "SyncInfo_SyncGroup");

            migrationBuilder.RenameColumn(
                name: "WorkItemSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "WorkItemSyncInfo_SyncGroup");

            migrationBuilder.RenameColumn(
                name: "UpdatesSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "WorkItemSyncInfo_ContentHash");

            migrationBuilder.RenameColumn(
                name: "RevisionsSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "UpdatesSyncInfo_SyncGroup");

            migrationBuilder.RenameColumn(
                name: "CommentsSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "UpdatesSyncInfo_ContentHash");

            migrationBuilder.RenameColumn(
                name: "SyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "SyncInfo_SyncGroup");

            migrationBuilder.AddColumn<string>(
                name: "SyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentsSyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentsSyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisionsSyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisionsSyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates");

            migrationBuilder.DropColumn(
                name: "CommentsSyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "CommentsSyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "RevisionsSyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "RevisionsSyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "SyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.RenameColumn(
                name: "SyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                newName: "SyncInfo_SyncHash");

            migrationBuilder.RenameColumn(
                name: "WorkItemSyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "WorkItemSyncInfo_SyncHash");

            migrationBuilder.RenameColumn(
                name: "WorkItemSyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "UpdatesSyncInfo_SyncHash");

            migrationBuilder.RenameColumn(
                name: "UpdatesSyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "RevisionsSyncInfo_SyncHash");

            migrationBuilder.RenameColumn(
                name: "UpdatesSyncInfo_ContentHash",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "CommentsSyncInfo_SyncHash");

            migrationBuilder.RenameColumn(
                name: "SyncInfo_SyncGroup",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "SyncInfo_SyncHash");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BadgeSyncInfo_LastSyncDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeSyncInfo_LastSyncOffset",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BadgeSyncInfo_SyncEndDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BadgeSyncInfo_SyncStartDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BadgeSyncInfo_SyncStatus",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DocumentSyncInfo_LastSyncDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentSyncInfo_LastSyncOffset",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DocumentSyncInfo_SyncEndDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentSyncInfo_SyncHash",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DocumentSyncInfo_SyncStartDateTime",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentSyncInfo_SyncStatus",
                schema: "CrystaLearn",
                table: "CrystaPrograms",
                type: "integer",
                nullable: true);
        }
    }
}
