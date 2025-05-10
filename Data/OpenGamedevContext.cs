using Microsoft.EntityFrameworkCore;
using OpenGamedev.Models;

namespace OpenGamedev.Data
{
    public class OpenGamedevContext(DbContextOptions<OpenGamedevContext> options) : DbContext(options)
    {
        public DbSet<OpenGamedev.Models.Project> Projects { get; set; } = default!;
        public DbSet<OpenGamedev.Models.Solution> Solutions { get; set; } = default!;
        public DbSet<OpenGamedev.Models.FeatureRequest> FeatureRequests { get; set; } = default!;
        public DbSet<OpenGamedev.Models.FeatureRequestVote> FeatureRequestVotes { get; set; } = default!;
        public DbSet<OpenGamedev.Models.SolutionVote> SolutionVotes { get; set; } = default!;
        public DbSet<OpenGamedev.Models.FeatureCategory> FeatureCategories { get; set; } = default!;
        public DbSet<OpenGamedev.Models.FeatureRequestDependency> FeatureRequestDependencies { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FeatureRequestVote>()
                .HasOne(fv => fv.FeatureRequest) 
                .WithMany(fr => fr.Votes) 
                .HasForeignKey(fv => fv.FeatureRequestId) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SolutionVote>()
                .HasOne(sv => sv.Solution)
                .WithMany(s => s.Votes)
                .HasForeignKey(sv => sv.SolutionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeatureRequestVote>()
                .HasOne(fv => fv.User)
                .WithMany(u => u.FeatureRequestVotes)
                .HasForeignKey(fv => fv.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SolutionVote>()
                .HasOne(sv => sv.User)
                .WithMany(u => u.SolutionVotes)
                .HasForeignKey(sv => sv.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeatureRequest>()
               .HasOne(fr => fr.Project)
               .WithMany(p => p.FeatureRequests) 
               .HasForeignKey(fr => fr.ProjectId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Solution>()
               .HasOne(s => s.FeatureRequest)
               .WithMany(fr => fr.Solutions)
               .HasForeignKey(s => s.FeatureRequestId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeatureRequest>()
               .HasOne(fr => fr.Author)
               .WithMany(u => u.CreatedFeatureRequests) 
               .HasForeignKey(fr => fr.AuthorId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Solution>()
               .HasOne(s => s.Author)
               .WithMany(u => u.CreatedSolutions) 
               .HasForeignKey(s => s.AuthorId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeatureRequest>()
                .HasOne(fr => fr.Category)
                .WithMany(c => c.FeatureRequests)
                .HasForeignKey(fr => fr.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<FeatureRequestDependency>()
                .HasOne(d => d.FeatureRequest)
                .WithMany(fr => fr.DependentOnTasks)
                .HasForeignKey(d => d.FeatureRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeatureRequestDependency>()
                .HasOne(d => d.DependsOnFeatureRequest)
                .WithMany(fr => fr.TasksDependingOnThis)
                .HasForeignKey(d => d.DependsOnFeatureRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeatureRequestVote>()
                .HasIndex(fv => new { fv.UserId, fv.FeatureRequestId })
                .IsUnique();

            modelBuilder.Entity<SolutionVote>()
                .HasIndex(sv => new { sv.UserId, sv.SolutionId })
                .IsUnique();

            modelBuilder.Entity<FeatureRequestDependency>()
               .HasIndex(d => new { d.FeatureRequestId, d.DependsOnFeatureRequestId })
               .IsUnique();
        }
    }
}
