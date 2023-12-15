using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystallineSociety.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Organizations_OrganizationId",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "EducationProgramId",
                table: "Badges");

            migrationBuilder.AddColumn<string>(
                name: "ProgramDocumentUrl",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProgramDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HtmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramDocument_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramDocument_OrganizationId",
                table: "ProgramDocument",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Organizations_OrganizationId",
                table: "Badges",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Organizations_OrganizationId",
                table: "Badges");

            migrationBuilder.DropTable(
                name: "ProgramDocument");

            migrationBuilder.DropColumn(
                name: "ProgramDocumentUrl",
                table: "Organizations");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EducationProgramId",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Organizations_OrganizationId",
                table: "Badges",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }
    }
}
