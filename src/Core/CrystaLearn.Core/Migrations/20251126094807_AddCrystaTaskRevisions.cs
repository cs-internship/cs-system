using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCrystaTaskRevisions : Migration
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

            migrationBuilder.CreateTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CrystaTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    RevisionCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
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
                    table.ForeignKey(
                        name: "FK_CrystaTaskRevisions_CrystaTasks_Id",
                        column: x => x.Id,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_CrystaTaskId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "CrystaTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrystaTaskRevisions",
                schema: "CrystaLearn");

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
