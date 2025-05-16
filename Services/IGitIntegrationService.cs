using OpenGamedev.Models;

namespace OpenGamedev.Services
{
    // This interface defines the contract for interacting with your Git repository.
    public interface IGitIntegrationService
    {
        /// <summary>
        /// Attempts to merge a source branch into the target branch for a specific repository.
        /// </summary>
        /// <param name="repositoryPath">The local file system path to the Git repository.</param>
        /// <param name="sourceBranch">The branch to merge from (e.g., solution branch).</param>
        /// <param name="targetBranch">The branch to merge into (e.g., main).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if merge was successful, false if there were merge conflicts.</returns>
        Task<bool> TryMergeBranchAsync(string repositoryPath, string sourceBranch, string targetBranch, CancellationToken cancellationToken); // Added repositoryPath

        /// <summary>
        /// Gets the current state/commit hash of a branch head for a specific repository.
        /// </summary>
        /// <param name="repositoryPath">The local file system path to the Git repository.</param>
        /// <param name="branchName">The name of the branch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The commit hash, or null if the branch is not found or an error occurs.</returns>
        Task<string?> GetBranchHeadAsync(string repositoryPath, string branchName, CancellationToken cancellationToken); // Added repositoryPath

        /// <summary>
        /// Checks if a given branch has merge conflicts with the target branch for a specific repository without merging.
        /// </summary>
        /// <param name="repositoryPath">The local file system path to the Git repository.</param>
        /// <param name="sourceBranch">The branch to check (e.g., solution branch).</param>
        /// <param name="targetBranch">The branch to check against (e.g., main).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if conflicts are detected, false otherwise.</returns>
        Task<bool> CheckForConflictsAsync(string repositoryPath, string sourceBranch, string targetBranch, CancellationToken cancellationToken); // Added repositoryPath

        /// <summary>
        /// Get the source code URL or branch name for a given solution.
        /// This method might need to access the Solution's Project to get the repository URL.
        /// </summary>
        /// <param name="solution">The Solution object.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The branch name, or null if it cannot be determined.</returns>
        Task<string?> GetSolutionBranchNameAsync(Solution solution, CancellationToken cancellationToken);

        // TODO: Add methods for cloning repositories, fetching branches, etc., also accepting repositoryPath
        // Task<bool> CloneRepositoryAsync(string remoteRepositoryUrl, string localPath, CancellationToken cancellationToken); // localPath is the result here
        // Task<bool> FetchBranchAsync(string repositoryPath, string branchName, CancellationToken cancellationToken);
        // Task<bool> CheckoutBranchAsync(string repositoryPath, string branchName, CancellationToken cancellationToken);
        // Task<bool> PullAsync(string repositoryPath, string branchName, CancellationToken cancellationToken);
        // Task<bool> PushAsync(string repositoryPath, string branchName, CancellationToken cancellationToken);
    }

}
