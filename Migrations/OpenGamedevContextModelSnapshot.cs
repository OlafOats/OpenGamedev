﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenGamedev.Data;

#nullable disable

namespace OpenGamedev.Migrations
{
    [DbContext(typeof(OpenGamedevContext))]
    partial class OpenGamedevContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OpenGamedev.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("FeatureCategories");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan?>("AverageSolutionVotingDuration")
                        .HasColumnType("time");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<long>("ProjectId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("SolutionVotingStartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long?>("SupersededByFeatureRequestId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("SupersededByFeatureRequestId");

                    b.ToTable("FeatureRequests");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequestDependency", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("DependsOnFeatureRequestId")
                        .HasColumnType("bigint");

                    b.Property<long>("FeatureRequestId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DependsOnFeatureRequestId");

                    b.HasIndex("FeatureRequestId", "DependsOnFeatureRequestId")
                        .IsUnique();

                    b.ToTable("FeatureRequestDependencies");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequestVote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("FeatureRequestId")
                        .HasColumnType("bigint");

                    b.Property<TimeSpan?>("SuggestedSolutionVotingDuration")
                        .HasColumnType("time");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VoteType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FeatureRequestId");

                    b.HasIndex("UserId", "FeatureRequestId")
                        .IsUnique();

                    b.ToTable("FeatureRequestVotes");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequestWorkArea", b =>
                {
                    b.Property<long>("FeatureRequestId")
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    b.Property<long>("WorkAreaId")
                        .HasColumnType("bigint")
                        .HasColumnOrder(2);

                    b.HasKey("FeatureRequestId", "WorkAreaId");

                    b.HasIndex("WorkAreaId");

                    b.ToTable("FeatureRequestWorkAreas");
                });

            modelBuilder.Entity("OpenGamedev.Models.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RepositoryLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("OpenGamedev.Models.Solution", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BuildArtifactUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BuildDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BuildLog")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FeatureRequestId")
                        .HasColumnType("bigint");

                    b.Property<bool>("HasMergeConflicts")
                        .HasColumnType("bit");

                    b.Property<string>("SourceCodeUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("FeatureRequestId");

                    b.ToTable("Solutions");
                });

            modelBuilder.Entity("OpenGamedev.Models.SolutionVote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("SolutionId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VoteType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SolutionId");

                    b.HasIndex("UserId", "SolutionId")
                        .IsUnique();

                    b.ToTable("SolutionVotes");
                });

            modelBuilder.Entity("OpenGamedev.Models.WorkArea", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("WorkAreas");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequest", b =>
                {
                    b.HasOne("OpenGamedev.Models.ApplicationUser", "Author")
                        .WithMany("CreatedFeatureRequests")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.FeatureCategory", "Category")
                        .WithMany("FeatureRequests")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.Project", "Project")
                        .WithMany("FeatureRequests")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.FeatureRequest", "SupersededBy")
                        .WithMany("TasksSupersededByThis")
                        .HasForeignKey("SupersededByFeatureRequestId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Author");

                    b.Navigation("Category");

                    b.Navigation("Project");

                    b.Navigation("SupersededBy");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequestDependency", b =>
                {
                    b.HasOne("OpenGamedev.Models.FeatureRequest", "DependsOnFeatureRequest")
                        .WithMany("TasksDependingOnThis")
                        .HasForeignKey("DependsOnFeatureRequestId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.FeatureRequest", "FeatureRequest")
                        .WithMany("DependentOnTasks")
                        .HasForeignKey("FeatureRequestId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DependsOnFeatureRequest");

                    b.Navigation("FeatureRequest");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequestVote", b =>
                {
                    b.HasOne("OpenGamedev.Models.FeatureRequest", "FeatureRequest")
                        .WithMany("Votes")
                        .HasForeignKey("FeatureRequestId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.ApplicationUser", "User")
                        .WithMany("FeatureRequestVotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FeatureRequest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequestWorkArea", b =>
                {
                    b.HasOne("OpenGamedev.Models.FeatureRequest", "FeatureRequest")
                        .WithMany("FeatureRequestWorkAreas")
                        .HasForeignKey("FeatureRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.WorkArea", "WorkArea")
                        .WithMany("FeatureRequestWorkAreas")
                        .HasForeignKey("WorkAreaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FeatureRequest");

                    b.Navigation("WorkArea");
                });

            modelBuilder.Entity("OpenGamedev.Models.Solution", b =>
                {
                    b.HasOne("OpenGamedev.Models.ApplicationUser", "Author")
                        .WithMany("CreatedSolutions")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.FeatureRequest", "FeatureRequest")
                        .WithMany("Solutions")
                        .HasForeignKey("FeatureRequestId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("FeatureRequest");
                });

            modelBuilder.Entity("OpenGamedev.Models.SolutionVote", b =>
                {
                    b.HasOne("OpenGamedev.Models.Solution", "Solution")
                        .WithMany("Votes")
                        .HasForeignKey("SolutionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenGamedev.Models.ApplicationUser", "User")
                        .WithMany("SolutionVotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Solution");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenGamedev.Models.ApplicationUser", b =>
                {
                    b.Navigation("CreatedFeatureRequests");

                    b.Navigation("CreatedSolutions");

                    b.Navigation("FeatureRequestVotes");

                    b.Navigation("SolutionVotes");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureCategory", b =>
                {
                    b.Navigation("FeatureRequests");
                });

            modelBuilder.Entity("OpenGamedev.Models.FeatureRequest", b =>
                {
                    b.Navigation("DependentOnTasks");

                    b.Navigation("FeatureRequestWorkAreas");

                    b.Navigation("Solutions");

                    b.Navigation("TasksDependingOnThis");

                    b.Navigation("TasksSupersededByThis");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("OpenGamedev.Models.Project", b =>
                {
                    b.Navigation("FeatureRequests");
                });

            modelBuilder.Entity("OpenGamedev.Models.Solution", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("OpenGamedev.Models.WorkArea", b =>
                {
                    b.Navigation("FeatureRequestWorkAreas");
                });
#pragma warning restore 612, 618
        }
    }
}
