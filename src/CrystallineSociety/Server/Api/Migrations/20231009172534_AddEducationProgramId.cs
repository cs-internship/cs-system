using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystallineSociety.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEducationProgramId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_EducationPrograms_EducationProgramId",
                table: "Badges");

            migrationBuilder.AlterColumn<Guid>(
                name: "EducationProgramId",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_EducationPrograms_EducationProgramId",
                table: "Badges",
                column: "EducationProgramId",
                principalTable: "EducationPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_EducationPrograms_EducationProgramId",
                table: "Badges");

            migrationBuilder.AlterColumn<Guid>(
                name: "EducationProgramId",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_EducationPrograms_EducationProgramId",
                table: "Badges",
                column: "EducationProgramId",
                principalTable: "EducationPrograms",
                principalColumn: "Id");
        }
    }
}
