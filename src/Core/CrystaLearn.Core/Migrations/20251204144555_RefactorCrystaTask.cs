using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCrystaTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_State",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "CreatedByDisplayName",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "DueDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "ResolvedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.RenameColumn(
                name: "StateChangeDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "TaskChangedDateTime");

            migrationBuilder.AlterColumn<string>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AttachmentsCount",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_Status",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_TaskCreateDateTime",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "TaskCreateDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_Status",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_TaskCreateDateTime",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.RenameColumn(
                name: "TaskChangedDateTime",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                newName: "StateChangeDate");

            migrationBuilder.AlterColumn<string>(
                name: "Revision",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttachmentsCount",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClosedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByDisplayName",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DueDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ResolvedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_ChangedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "ChangedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_State",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "State");
        }
    }
}
