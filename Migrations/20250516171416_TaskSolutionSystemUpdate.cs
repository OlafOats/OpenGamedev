using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenGamedev.Migrations
{
    /// <inheritdoc />
    public partial class TaskSolutionSystemUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMergeConflicts",
                table: "Solutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "FeatureRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasMergeConflicts",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "FeatureRequests");
        }
    }
}
