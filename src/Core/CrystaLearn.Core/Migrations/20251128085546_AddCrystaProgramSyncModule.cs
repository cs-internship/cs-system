using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCrystaProgramSyncModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrystaProgramSyncModules",
                schema: "CrystaLearn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CrystaProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModuleType = table.Column<int>(type: "integer", nullable: false),
                    SyncConfig = table.Column<string>(type: "text", nullable: true),
                    SyncInfo_SyncId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_SyncEndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_ContentHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SyncInfo_SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    SyncInfo_LastSyncDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncInfo_LastSyncOffset = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrystaProgramSyncModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrystaProgramSyncModules_CrystaPrograms_CrystaProgramId",
                        column: x => x.CrystaProgramId,
                        principalSchema: "CrystaLearn",
                        principalTable: "CrystaPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrystaProgramSyncModules_CrystaProgramId",
                schema: "CrystaLearn",
                table: "CrystaProgramSyncModules",
                column: "CrystaProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_CrystaProgramSyncModules_ModuleType",
                schema: "CrystaLearn",
                table: "CrystaProgramSyncModules",
                column: "ModuleType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrystaProgramSyncModules",
                schema: "CrystaLearn");
        }
    }
}
