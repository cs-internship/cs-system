using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystallineSociety.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameEducationToOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_EducationPrograms_EducationProgramId",
                table: "Badges");

            migrationBuilder.DropTable(
                name: "EducationPrograms");

            migrationBuilder.DropIndex(
                name: "IX_Badges_EducationProgramId",
                table: "Badges");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BadgeSystemUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSyncDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastCommitHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_OrganizationId",
                table: "Badges",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Organizations_OrganizationId",
                table: "Badges",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Organizations_OrganizationId",
                table: "Badges");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Badges_OrganizationId",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Badges");

            migrationBuilder.CreateTable(
                name: "EducationPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeSystemUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastCommitHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastSyncDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationPrograms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_EducationProgramId",
                table: "Badges",
                column: "EducationProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_EducationPrograms_EducationProgramId",
                table: "Badges",
                column: "EducationProgramId",
                principalTable: "EducationPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
