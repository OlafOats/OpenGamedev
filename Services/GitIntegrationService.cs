using LibGit2Sharp;
using OpenGamedev.Models;

namespace OpenGamedev.Services
{

    public class GitIntegrationService(ILogger<GitIntegrationService> logger /*, IConfiguration configuration */) : IGitIntegrationService
    {
        private readonly ILogger<GitIntegrationService> _logger = logger;
        public Task<bool> TryMergeBranchAsync(string repositoryPath, string sourceBranch, string targetBranch, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to merge branch '{SourceBranch}' into '{TargetBranch}' in repository '{RepositoryPath}'.", sourceBranch, targetBranch, repositoryPath);

            if (!Repository.IsValid(repositoryPath))
            {
                _logger.LogError("Invalid Git repository path: {RepositoryPath}.", repositoryPath);
                return Task.FromResult(false);
            }

            try
            {
                using var repo = new Repository(repositoryPath);

                // Ensure the working directory is clean
                if (repo.RetrieveStatus().IsDirty)
                {
                    _logger.LogError("Working directory is not clean in repository '{RepositoryPath}'.", repositoryPath);
                    return Task.FromResult(false);
                }

                // Checkout the target branch
                Commands.Checkout(repo, targetBranch);

                // Fetch the source branch
                var sourceBranchRef = repo.Branches[sourceBranch];
                if (sourceBranchRef == null)
                {
                    _logger.LogError("Source branch '{SourceBranch}' not found in repository '{RepositoryPath}'.", sourceBranch, repositoryPath);
                    return Task.FromResult(false);
                }

                // Perform the merge
                var signature = new Signature("OpenGamedev Bot", "bot@opengamedev.com", DateTimeOffset.Now);
                var mergeResult = repo.Merge(sourceBranchRef, signature);

                switch (mergeResult.Status)
                {
                    case MergeStatus.Conflicts:
                        _logger.LogWarning("Merge conflicts detected when merging '{SourceBranch}' into '{TargetBranch}' in '{RepositoryPath}'.", sourceBranch, targetBranch, repositoryPath);
                        return Task.FromResult(false);

                    case MergeStatus.UpToDate:
                        _logger.LogInformation("Branches are already up-to-date.");
                        return Task.FromResult(true);

                    case MergeStatus.FastForward:
                    case MergeStatus.NonFastForward:
                        _logger.LogInformation("Merge completed successfully.");
                        return Task.FromResult(true);

                    default:
                        _logger.LogError("Unexpected merge status: {MergeStatus}.", mergeResult.Status);
                        return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the merge operation.");
                return Task.FromResult(false);
            }
        }

        public Task<string?> GetBranchHeadAsync(string repositoryPath, string branchName, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting head commit for branch '{BranchName}' in repository '{RepositoryPath}' using LibGit2Sharp.", branchName, repositoryPath);

            if (!Repository.IsValid(repositoryPath))
            {
                _logger.LogError("Invalid Git repository path: {RepositoryPath}.", repositoryPath);
                return Task.FromResult<string?>(null);
            }

            try
            {
                using var repo = new Repository(repositoryPath);

                var branch = repo.Branches[branchName];
                if (branch == null)
                {
                    _logger.LogWarning("Branch '{BranchName}' not found in repository '{RepositoryPath}'.", branchName, repositoryPath);
                    return Task.FromResult<string?>(null);
                }

                var commitHash = branch.Tip?.Sha;
                _logger.LogInformation("Head commit for branch '{BranchName}' is '{CommitHash}'.", branchName, commitHash);
                return Task.FromResult(commitHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the branch head.");
                return Task.FromResult<string?>(null);
            }
        }

        public Task<bool> CheckForConflictsAsync(string repositoryPath, string sourceBranch, string targetBranch, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking for conflicts between branch '{SourceBranch}' and '{TargetBranch}' in repository '{RepositoryPath}' using LibGit2Sharp.", sourceBranch, targetBranch, repositoryPath);

            if (!Repository.IsValid(repositoryPath))
            {
                _logger.LogError("Invalid Git repository path: {RepositoryPath}.", repositoryPath);
                return Task.FromResult(false);
            }

            try
            {
                using var repo = new Repository(repositoryPath);

                // Checkout the target branch
                Commands.Checkout(repo, targetBranch);

                // Fetch the source branch
                var sourceBranchRef = repo.Branches[sourceBranch];
                if (sourceBranchRef == null)
                {
                    _logger.LogError("Source branch '{SourceBranch}' not found in repository '{RepositoryPath}'.", sourceBranch, repositoryPath);
                    return Task.FromResult(false);
                }

                // Perform a merge in dry-run mode
                var signature = new Signature("OpenGamedev Bot", "bot@opengamedev.com", DateTimeOffset.Now);
                var mergeResult = repo.Merge(sourceBranchRef, signature, new MergeOptions { CommitOnSuccess = false });

                if (mergeResult.Status == MergeStatus.Conflicts)
                {
                    _logger.LogWarning("Conflicts detected between '{SourceBranch}' and '{TargetBranch}'.", sourceBranch, targetBranch);
                    return Task.FromResult(true); // Conflicts detected
                }

                _logger.LogInformation("No conflicts detected.");
                return Task.FromResult(false); // No conflicts
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the conflict check.");
                return Task.FromResult(false);
            }
        }
        /// <summary>
        /// Derives the source branch name from a Solution object.
        /// This part is based on your application's convention, not Git itself.
        /// </summary>
        /// <param name="solution">The Solution object.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The branch name, or null if it cannot be determined.</returns>
        public Task<string?> GetSolutionBranchNameAsync(Solution solution, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to derive branch name for Solution {SolutionId}.", solution.Id);

            // --- IMPLEMENT YOUR LOGIC HERE TO GET THE ACTUAL BRANCH NAME FROM THE SOLUTION ---
            // Example: Extract branch name from Solution.SourceCodeUrl if it follows a specific pattern
            if (!string.IsNullOrEmpty(solution.SourceCodeUrl) && Uri.TryCreate(solution.SourceCodeUrl, UriKind.Absolute, out var uri))
            {
                var segments = uri.Segments;
                // Example: URL like https://github.com/user/repo/tree/branch-name
                if (segments.Length > 3 && segments[^2].Trim('/') == "tree")
                {
                    string branchName = segments.Last().Trim('/');
                    _logger.LogInformation("Derived branch name '{BranchName}' from SourceCodeUrl.", branchName);
                    return Task.FromResult<string?>(branchName);
                }
            }

            // Example: If Solution model has a dedicated BranchName property
            // if (!string.IsNullOrEmpty(solution.BranchName))
            // {
            //     _logger.LogInformation("Using BranchName property from Solution {SolutionId}: {BranchName}", solution.Id, solution.BranchName);
            //     return Task.FromResult<string?>(solution.BranchName);
            // }

            // Example: Fallback to a naming convention
            // string fallbackBranchName = $"solution-{solution.Id}-branch";
            // _logger.LogWarning("Could not derive branch name for Solution {SolutionId}. Using fallback convention: {FallbackBranchName}", solution.Id, fallbackBranchName);
            // return Task.FromResult<string?>(fallbackBranchName);

            _logger.LogWarning("Could not determine branch name for Solution {SolutionId}.", solution.Id);
            return Task.FromResult<string?>(null); // Return null if branch name cannot be determined
            // --- END IMPLEMENTATION ---
        }
        public Task<string> CloneOrUpdateRepositoryAsync(string repositoryUrl, string localPath)
        {
            _logger.LogInformation("Cloning or updating repository '{RepositoryUrl}' at '{LocalPath}'.", repositoryUrl, localPath);

            try
            {
                if (Directory.Exists(localPath) && Repository.IsValid(localPath))
                {
                    // Update existing repository
                    using var repo = new Repository(localPath);
                    Commands.Fetch(repo, "origin", [], new FetchOptions(), null);
                    _logger.LogInformation("Repository at '{LocalPath}' updated successfully.", localPath);
                }
                else
                {
                    // Clone repository
                    if (Directory.Exists(localPath))
                    {
                        Directory.Delete(localPath, true); // Clean up if directory exists but is not a valid repo
                    }

                    Repository.Clone(repositoryUrl, localPath);
                    _logger.LogInformation("Repository cloned successfully from '{RepositoryUrl}' to '{LocalPath}'.", repositoryUrl, localPath);
                }

                return Task.FromResult(localPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cloning or updating the repository.");
                throw;
            }
        }
        public Task<bool> PushChangesAsync(string repositoryPath, string branchName)
        {
            _logger.LogInformation("Pushing changes for branch '{BranchName}' in repository '{RepositoryPath}'.", branchName, repositoryPath);

            if (!Repository.IsValid(repositoryPath))
            {
                _logger.LogError("Invalid Git repository path: {RepositoryPath}.", repositoryPath);
                return Task.FromResult(false);
            }

            try
            {
                using var repo = new Repository(repositoryPath);

                var remote = repo.Network.Remotes["origin"];
                var pushOptions = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials
                        {
                            Username = "your-username", // Replace with actual username
                            Password = "your-password"  // Replace with actual password or token
                        }
                };

                repo.Network.Push(remote, $"refs/heads/{branchName}", pushOptions);
                _logger.LogInformation("Changes pushed successfully for branch '{BranchName}' in repository '{RepositoryPath}'.", branchName, repositoryPath);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while pushing changes.");
                return Task.FromResult(false);
            }
        }
        public bool IsWorkingDirectoryClean(Repository repo)
        {
            var status = repo.RetrieveStatus();
            return !status.IsDirty;
        }
    }
}

