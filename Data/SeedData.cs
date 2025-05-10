using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Models;

namespace OpenGamedev.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(OpenGamedevContext context, UserManager<ApplicationUser> userManager)
        {
            await context.Database.MigrateAsync();

            if (!await context.Projects.AnyAsync())
            {
                // Логика, аналогичная твоей из Index.cshtml.cs:
                var firstUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == "0");
                firstUser ??= new ApplicationUser { UserName = "Default" };

                var gameDesign = await context.FeatureCategories.FirstOrDefaultAsync(fc => fc.Name == "Game design");
                gameDesign ??= new FeatureCategory { Name = "Game design" };

                var defaultProject = new Project
                {
                    Title = "OpenGamedev Default Project",
                    Description = "The default project for community game development.",
                    CreationDate = DateTime.UtcNow,
                };

                context.Projects.Add(defaultProject);
                await context.SaveChangesAsync();

                var initialFeatureRequest = new FeatureRequest
                {
                    ProjectId = defaultProject.Id,
                    Project = defaultProject,
                    CategoryId = gameDesign.Id,
                    Category = gameDesign,
                    Title = "Придумать концепт игры",
                    Description = "Предложите идеи для базового концепта игры, которую мы будем разрабатывать.",
                    CreationDate = DateTime.UtcNow,
                    Status = FeatureRequestStatus.InProgress,
                    AuthorId = firstUser.Id,
                    Author = firstUser,
                    AverageSolutionVotingDuration = TimeSpan.FromDays(7),
                    SolutionVotingStartTime = DateTime.UtcNow,
                };

                context.FeatureRequests.Add(initialFeatureRequest);
                await context.SaveChangesAsync();
            }
        }
    }

}
