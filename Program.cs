using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;
using Microsoft.AspNetCore.Identity;
using OpenGamedev.Services;
using OpenGamedev.Services.Interfaces;
using OpenGamedev.Services.Processors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<OpenGamedevContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OpenGamedevContext")
                          ?? throw new InvalidOperationException("Connection string 'OpenGamedevContext' not found.")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<OpenGamedevContext>();

builder.Services.AddSingleton<IGitIntegrationService, GitIntegrationService>();

builder.Services.AddSingleton<INotificationService, DummyNotificationService>();

builder.Services.AddScoped<IDependencyProcessor, DependencyProcessor>();

builder.Services.AddScoped<ITaskVotingProcessor, TaskVotingProcessor>();

builder.Services.AddScoped<ISolutionVotingProcessor, SolutionVotingProcessor>();

builder.Services.AddScoped<IAcceptedSolutionProcessor, AcceptedSolutionProcessor>();

builder.Services.AddScoped<IConflictChecker, ConflictChecker>();

builder.Services.AddScoped<OpenGamedevContext>();

builder.Services.AddHostedService<TaskQueueBackgroundService>();


var app = builder.Build();

var env = app.Environment;

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<OpenGamedevContext>();
        context.Database.Migrate();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        await SeedData.InitializeAsync(context, userManager, env);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB or applying migrations.");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
