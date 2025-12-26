using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCrystaDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrystaDocument",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Code = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Culture = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SourceHtmlUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    SourceContentUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CrystaUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Folder = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    FileName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    LastHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_ContentHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    SyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CrystaProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    FileExtension = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FileNameWithoutExtension = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrystaDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrystaDocument_CrystaPrograms_CrystaProgramId",
                        column: x => x.CrystaProgramId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaPrograms",
                        principalColumn: "Id");
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CrystaDocument_Code",
            //    schema: "CrystaLearn",
            //    table: "CrystaDocument",
            //    column: "Code",
            //    unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaDocument_CrystaProgramId",
                schema: "CrystaLearn",
                table: "CrystaDocument",
                column: "CrystaProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaDocument_IsActive",
                schema: "CrystaLearn",
                table: "CrystaDocument",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaDocument_SyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaDocument",
                column: "SyncInfo_SyncId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrystaDocument",
                schema: "CrystaLearn");
        }
    }
}
