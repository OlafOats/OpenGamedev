using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenGamedev.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkAreas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SupersededByFeatureRequestId",
                table: "FeatureRequests",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkAreas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureRequestWorkAreas",
                columns: table => new
                {
                    FeatureRequestId = table.Column<long>(type: "bigint", nullable: false),
                    WorkAreaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureRequestWorkAreas", x => new { x.FeatureRequestId, x.WorkAreaId });
                    table.ForeignKey(
                        name: "FK_FeatureRequestWorkAreas_FeatureRequests_FeatureRequestId",
                        column: x => x.FeatureRequestId,
                        principalTable: "FeatureRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureRequestWorkAreas_WorkAreas_WorkAreaId",
                        column: x => x.WorkAreaId,
                        principalTable: "WorkAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRequests_SupersededByFeatureRequestId",
                table: "FeatureRequests",
                column: "SupersededByFeatureRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRequestWorkAreas_WorkAreaId",
                table: "FeatureRequestWorkAreas",
                column: "WorkAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequests_FeatureRequests_SupersededByFeatureRequestId",
                table: "FeatureRequests",
                column: "SupersededByFeatureRequestId",
                principalTable: "FeatureRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequests_FeatureRequests_SupersededByFeatureRequestId",
                table: "FeatureRequests");

            migrationBuilder.DropTable(
                name: "FeatureRequestWorkAreas");

            migrationBuilder.DropTable(
                name: "WorkAreas");

            migrationBuilder.DropIndex(
                name: "IX_FeatureRequests_SupersededByFeatureRequestId",
                table: "FeatureRequests");

            migrationBuilder.DropColumn(
                name: "SupersededByFeatureRequestId",
                table: "FeatureRequests");
        }
    }
}
