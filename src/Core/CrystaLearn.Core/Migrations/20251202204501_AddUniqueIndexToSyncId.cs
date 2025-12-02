using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToSyncId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskUpdates_SyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates",
                column: "SyncInfo_SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTasks_WorkItemSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTasks",
                column: "WorkItemSyncInfo_SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskRevisions_WorkItemSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions",
                column: "WorkItemSyncInfo_SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrystaTaskComments_SyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments",
                column: "SyncInfo_SyncId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskUpdates_SyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTaskUpdates");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTasks_WorkItemSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTasks");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskRevisions_WorkItemSyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTaskRevisions");

            migrationBuilder.DropIndex(
                name: "IX_CrystaTaskComments_SyncInfo_SyncId",
                schema: "CrystaLearn",
                table: "CrystaTaskComments");
        }
    }
}
