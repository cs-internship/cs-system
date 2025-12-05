using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCrystaCommentTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskComments_CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.RenameColumn(
                name: "EditedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "EditedDateTime");

            migrationBuilder.RenameColumn(
                name: "EditedBy",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "EditedByText");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "CreatedByText");

            migrationBuilder.DropColumn(
             name: "EditedById",
             schema: "CrystaLearn",
             table: "CrystaTaskComments");

            migrationBuilder.DropColumn(
            name: "CreatedById",
            schema: "CrystaLearn",
            table: "CrystaTaskComments");

            migrationBuilder.AddColumn<Guid>(
                name: "EditedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDateTime",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_CreatedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_CreatedDateTime",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "CreatedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_EditedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "EditedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CrystaTaskComments_Users_CreatedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "CreatedById",
                principalSchema: "CrystaLearn",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrystaTaskComments_Users_EditedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "EditedById",
                principalSchema: "CrystaLearn",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrystaTaskComments_Users_CreatedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_CrystaTaskComments_Users_EditedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskComments_CreatedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskComments_CreatedDateTime",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskComments_EditedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");

            migrationBuilder.RenameColumn(
                name: "EditedDateTime",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "EditedDate");

            migrationBuilder.RenameColumn(
                name: "EditedByText",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "EditedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByText",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "EditedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_CreatedDate",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "CreatedDate");
        }
    }
}
