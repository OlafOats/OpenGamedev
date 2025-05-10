using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenGamedev.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesAndDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "SuggestedSolutionVotingDuration",
                table: "FeatureRequestVote",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AverageSolutionVotingDuration",
                table: "FeatureRequest",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "FeatureRequest",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "SolutionVotingStartTime",
                table: "FeatureRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeatureCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureRequestDependencies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureRequestId = table.Column<long>(type: "bigint", nullable: false),
                    DependsOnFeatureRequestId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureRequestDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureRequestDependencies_FeatureRequest_DependsOnFeatureRequestId",
                        column: x => x.DependsOnFeatureRequestId,
                        principalTable: "FeatureRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeatureRequestDependencies_FeatureRequest_FeatureRequestId",
                        column: x => x.FeatureRequestId,
                        principalTable: "FeatureRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRequest_CategoryId",
                table: "FeatureRequest",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRequestDependencies_DependsOnFeatureRequestId",
                table: "FeatureRequestDependencies",
                column: "DependsOnFeatureRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRequestDependencies_FeatureRequestId_DependsOnFeatureRequestId",
                table: "FeatureRequestDependencies",
                columns: new[] { "FeatureRequestId", "DependsOnFeatureRequestId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequest_FeatureCategory_CategoryId",
                table: "FeatureRequest",
                column: "CategoryId",
                principalTable: "FeatureCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequest_FeatureCategory_CategoryId",
                table: "FeatureRequest");

            migrationBuilder.DropTable(
                name: "FeatureCategory");

            migrationBuilder.DropTable(
                name: "FeatureRequestDependencies");

            migrationBuilder.DropIndex(
                name: "IX_FeatureRequest_CategoryId",
                table: "FeatureRequest");

            migrationBuilder.DropColumn(
                name: "SuggestedSolutionVotingDuration",
                table: "FeatureRequestVote");

            migrationBuilder.DropColumn(
                name: "AverageSolutionVotingDuration",
                table: "FeatureRequest");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FeatureRequest");

            migrationBuilder.DropColumn(
                name: "SolutionVotingStartTime",
                table: "FeatureRequest");
        }
    }
}
