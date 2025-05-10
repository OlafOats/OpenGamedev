using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenGamedev.Migrations
{
    /// <inheritdoc />
    public partial class TablesRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequest_ApplicationUser_AuthorId",
                table: "FeatureRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequest_FeatureCategory_CategoryId",
                table: "FeatureRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequest_Project_ProjectId",
                table: "FeatureRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequest_DependsOnFeatureRequestId",
                table: "FeatureRequestDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequest_FeatureRequestId",
                table: "FeatureRequestDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestVote_ApplicationUser_UserId",
                table: "FeatureRequestVote");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestVote_FeatureRequest_FeatureRequestId",
                table: "FeatureRequestVote");

            migrationBuilder.DropForeignKey(
                name: "FK_Solution_ApplicationUser_AuthorId",
                table: "Solution");

            migrationBuilder.DropForeignKey(
                name: "FK_Solution_FeatureRequest_FeatureRequestId",
                table: "Solution");

            migrationBuilder.DropForeignKey(
                name: "FK_SolutionVote_ApplicationUser_UserId",
                table: "SolutionVote");

            migrationBuilder.DropForeignKey(
                name: "FK_SolutionVote_Solution_SolutionId",
                table: "SolutionVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolutionVote",
                table: "SolutionVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solution",
                table: "Solution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureRequestVote",
                table: "FeatureRequestVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureRequest",
                table: "FeatureRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCategory",
                table: "FeatureCategory");

            migrationBuilder.RenameTable(
                name: "SolutionVote",
                newName: "SolutionVotes");

            migrationBuilder.RenameTable(
                name: "Solution",
                newName: "Solutions");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "FeatureRequestVote",
                newName: "FeatureRequestVotes");

            migrationBuilder.RenameTable(
                name: "FeatureRequest",
                newName: "FeatureRequests");

            migrationBuilder.RenameTable(
                name: "FeatureCategory",
                newName: "FeatureCategories");

            migrationBuilder.RenameIndex(
                name: "IX_SolutionVote_UserId_SolutionId",
                table: "SolutionVotes",
                newName: "IX_SolutionVotes_UserId_SolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_SolutionVote_SolutionId",
                table: "SolutionVotes",
                newName: "IX_SolutionVotes_SolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Solution_FeatureRequestId",
                table: "Solutions",
                newName: "IX_Solutions_FeatureRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Solution_AuthorId",
                table: "Solutions",
                newName: "IX_Solutions_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequestVote_UserId_FeatureRequestId",
                table: "FeatureRequestVotes",
                newName: "IX_FeatureRequestVotes_UserId_FeatureRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequestVote_FeatureRequestId",
                table: "FeatureRequestVotes",
                newName: "IX_FeatureRequestVotes_FeatureRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequest_ProjectId",
                table: "FeatureRequests",
                newName: "IX_FeatureRequests_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequest_CategoryId",
                table: "FeatureRequests",
                newName: "IX_FeatureRequests_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequest_AuthorId",
                table: "FeatureRequests",
                newName: "IX_FeatureRequests_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolutionVotes",
                table: "SolutionVotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solutions",
                table: "Solutions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureRequestVotes",
                table: "FeatureRequestVotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureRequests",
                table: "FeatureRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCategories",
                table: "FeatureCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequests_DependsOnFeatureRequestId",
                table: "FeatureRequestDependencies",
                column: "DependsOnFeatureRequestId",
                principalTable: "FeatureRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequests_FeatureRequestId",
                table: "FeatureRequestDependencies",
                column: "FeatureRequestId",
                principalTable: "FeatureRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequests_ApplicationUser_AuthorId",
                table: "FeatureRequests",
                column: "AuthorId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequests_FeatureCategories_CategoryId",
                table: "FeatureRequests",
                column: "CategoryId",
                principalTable: "FeatureCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequests_Projects_ProjectId",
                table: "FeatureRequests",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestVotes_ApplicationUser_UserId",
                table: "FeatureRequestVotes",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestVotes_FeatureRequests_FeatureRequestId",
                table: "FeatureRequestVotes",
                column: "FeatureRequestId",
                principalTable: "FeatureRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_ApplicationUser_AuthorId",
                table: "Solutions",
                column: "AuthorId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_FeatureRequests_FeatureRequestId",
                table: "Solutions",
                column: "FeatureRequestId",
                principalTable: "FeatureRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolutionVotes_ApplicationUser_UserId",
                table: "SolutionVotes",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolutionVotes_Solutions_SolutionId",
                table: "SolutionVotes",
                column: "SolutionId",
                principalTable: "Solutions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequests_DependsOnFeatureRequestId",
                table: "FeatureRequestDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequests_FeatureRequestId",
                table: "FeatureRequestDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequests_ApplicationUser_AuthorId",
                table: "FeatureRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequests_FeatureCategories_CategoryId",
                table: "FeatureRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequests_Projects_ProjectId",
                table: "FeatureRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestVotes_ApplicationUser_UserId",
                table: "FeatureRequestVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureRequestVotes_FeatureRequests_FeatureRequestId",
                table: "FeatureRequestVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_ApplicationUser_AuthorId",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_FeatureRequests_FeatureRequestId",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_SolutionVotes_ApplicationUser_UserId",
                table: "SolutionVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_SolutionVotes_Solutions_SolutionId",
                table: "SolutionVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolutionVotes",
                table: "SolutionVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solutions",
                table: "Solutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureRequestVotes",
                table: "FeatureRequestVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureRequests",
                table: "FeatureRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCategories",
                table: "FeatureCategories");

            migrationBuilder.RenameTable(
                name: "SolutionVotes",
                newName: "SolutionVote");

            migrationBuilder.RenameTable(
                name: "Solutions",
                newName: "Solution");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameTable(
                name: "FeatureRequestVotes",
                newName: "FeatureRequestVote");

            migrationBuilder.RenameTable(
                name: "FeatureRequests",
                newName: "FeatureRequest");

            migrationBuilder.RenameTable(
                name: "FeatureCategories",
                newName: "FeatureCategory");

            migrationBuilder.RenameIndex(
                name: "IX_SolutionVotes_UserId_SolutionId",
                table: "SolutionVote",
                newName: "IX_SolutionVote_UserId_SolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_SolutionVotes_SolutionId",
                table: "SolutionVote",
                newName: "IX_SolutionVote_SolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_FeatureRequestId",
                table: "Solution",
                newName: "IX_Solution_FeatureRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_AuthorId",
                table: "Solution",
                newName: "IX_Solution_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequestVotes_UserId_FeatureRequestId",
                table: "FeatureRequestVote",
                newName: "IX_FeatureRequestVote_UserId_FeatureRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequestVotes_FeatureRequestId",
                table: "FeatureRequestVote",
                newName: "IX_FeatureRequestVote_FeatureRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequests_ProjectId",
                table: "FeatureRequest",
                newName: "IX_FeatureRequest_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequests_CategoryId",
                table: "FeatureRequest",
                newName: "IX_FeatureRequest_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureRequests_AuthorId",
                table: "FeatureRequest",
                newName: "IX_FeatureRequest_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolutionVote",
                table: "SolutionVote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solution",
                table: "Solution",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureRequestVote",
                table: "FeatureRequestVote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureRequest",
                table: "FeatureRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCategory",
                table: "FeatureCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequest_ApplicationUser_AuthorId",
                table: "FeatureRequest",
                column: "AuthorId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequest_FeatureCategory_CategoryId",
                table: "FeatureRequest",
                column: "CategoryId",
                principalTable: "FeatureCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequest_Project_ProjectId",
                table: "FeatureRequest",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequest_DependsOnFeatureRequestId",
                table: "FeatureRequestDependencies",
                column: "DependsOnFeatureRequestId",
                principalTable: "FeatureRequest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestDependencies_FeatureRequest_FeatureRequestId",
                table: "FeatureRequestDependencies",
                column: "FeatureRequestId",
                principalTable: "FeatureRequest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestVote_ApplicationUser_UserId",
                table: "FeatureRequestVote",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureRequestVote_FeatureRequest_FeatureRequestId",
                table: "FeatureRequestVote",
                column: "FeatureRequestId",
                principalTable: "FeatureRequest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solution_ApplicationUser_AuthorId",
                table: "Solution",
                column: "AuthorId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solution_FeatureRequest_FeatureRequestId",
                table: "Solution",
                column: "FeatureRequestId",
                principalTable: "FeatureRequest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolutionVote_ApplicationUser_UserId",
                table: "SolutionVote",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolutionVote_Solution_SolutionId",
                table: "SolutionVote",
                column: "SolutionId",
                principalTable: "Solution",
                principalColumn: "Id");
        }
    }
}
