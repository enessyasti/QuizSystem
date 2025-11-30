using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
// Set default culture to English (US)
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed roles, demo users, exercise types, and sample sessions for variety
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { "Admin", "User" };
    foreach (var r in roles)
    {
        if (!roleManager.RoleExistsAsync(r).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(r)).GetAwaiter().GetResult();
        }
    }

    ApplicationUser EnsureUser(string email, string password, string role)
    {
        var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
        if (user == null)
        {
            user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var createResult = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            if (!createResult.Succeeded)
            {
                // If creation failed due to password policy etc., you can adjust the password or policies.
                // For now, just skip.
            }
        }
        if (!userManager.IsInRoleAsync(user, role).GetAwaiter().GetResult())
        {
            userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
        }
        return user;
    }

    var admin = EnsureUser("admin@fitverse.com", "A!dm1n.2025", "Admin");
    var member = EnsureUser("member@fitverse.com", "M3mb3r.2025", "User");

    // Always ensure desired exercise types and session diversity
    if (true)
    {
        // Ensure desired exercise types exist (run every time to add missing ones)
        var desiredTypes = new[]
        {
            "Yoga", "Pilates", "Boxing", "Swimming", "Cycling", "Running",
            "CrossFit", "Powerlifting", "Rowing", "Basketball",
            "HIIT", "Stretching", "Martial Arts", "Tennis", "Football",
            "Badminton", "Volleyball", "Table Tennis", "Climbing", "Skating"
        };
        var addedAnyType = false;
        foreach (var name in desiredTypes)
        {
            if (!context.ExerciseTypes.Any(e => e.Name == name))
            {
                context.ExerciseTypes.Add(new ExerciseType { Name = name });
                addedAnyType = true;
            }
        }
        if (addedAnyType)
        {
            context.SaveChanges();
        }
    
        // Ensure the member has a variety of training sessions (past and future)
        const int targetSessionCount = 12;
        var existingCount = context.TrainingSessions.Count(ts => ts.UserId == member.Id);
        if (existingCount < targetSessionCount)
        {
            var sessionsToAdd = new List<TrainingSession>();
            var baseYear = DateTime.Now.Year;
            var days = new[]
            {
                new DateTime(baseYear, 12, 1),
                new DateTime(baseYear, 12, 2),
                new DateTime(baseYear, 12, 3),
                new DateTime(baseYear, 12, 4),
                new DateTime(baseYear, 12, 5),
                new DateTime(baseYear, 12, 6),
                new DateTime(baseYear, 12, 7)
            };
            var dayHours = new[] { 8, 11, 14, 17, 20 };

            int toCreate = targetSessionCount - existingCount;
            for (int i = 0; i < toCreate; i++)
            {
                var day = days[i % days.Length];
                var hour = dayHours[(i + 1) % dayHours.Length];
                var start = day.AddHours(hour).AddMinutes((i * 9) % 55);
                var end = start.AddMinutes(45 + (i * 7) % 35);
                sessionsToAdd.Add(new TrainingSession
                {
                    UserId = member.Id,
                    StartTime = start,
                    EndTime = end
                });
            }

            if (sessionsToAdd.Count > 0)
            {
                context.TrainingSessions.AddRange(sessionsToAdd);
                context.SaveChanges();

                var types = context.ExerciseTypes.ToList();
                var rnd = new Random();
                foreach (var s in sessionsToAdd)
                {
                    // Add 1-3 exercises per session for diversity
                    int exerciseCount = 1 + rnd.Next(0, 3);
                    for (int j = 0; j < exerciseCount; j++)
                    {
                        var type = types[rnd.Next(types.Count)];
                        context.ExercisePerformeds.Add(new ExercisePerformed
                        {
                            TrainingSessionId = s.Id,
                            ExerciseTypeId = type.Id,
                            Load = rnd.Next(10, 120),
                            Sets = rnd.Next(3, 6),
                            Repetitions = rnd.Next(6, 15)
                        });
                    }
                }
                context.SaveChanges();
            }
        }
    }

    // Re-map dates of all member sessions to the week Dec 1-7 with varied daytime hours
    var allMemberSessions = context.TrainingSessions.Where(ts => ts.UserId == member.Id).OrderBy(ts => ts.Id).ToList();
    if (allMemberSessions.Any())
    {
        var baseYear = DateTime.Now.Year;
        var days = new[]
        {
            new DateTime(baseYear, 12, 1),
            new DateTime(baseYear, 12, 2),
            new DateTime(baseYear, 12, 3),
            new DateTime(baseYear, 12, 4),
            new DateTime(baseYear, 12, 5),
            new DateTime(baseYear, 12, 6),
            new DateTime(baseYear, 12, 7)
        };
        var dayHours = new[] { 8, 11, 14, 17, 20 };

        for (int i = 0; i < allMemberSessions.Count; i++)
        {
            var s = allMemberSessions[i];
            var day = days[i % days.Length];
            var hour = dayHours[(i + 2) % dayHours.Length];
            var start = day.AddHours(hour).AddMinutes((i * 11) % 50);
            var end = start.AddMinutes(40 + ((i * 3) % 20));
            s.StartTime = start;
            s.EndTime = end;
        }
        context.SaveChanges();
    }
    if (!context.TrainingSessions.Any(ts => ts.UserId == member.Id))
    {
        var sessions = new[]
        {
            new TrainingSession { UserId = member.Id, StartTime = DateTime.Now.AddDays(-14).Date.AddHours(6), EndTime = DateTime.Now.AddDays(-14).Date.AddHours(7) },
            new TrainingSession { UserId = member.Id, StartTime = DateTime.Now.AddDays(-7).Date.AddHours(18).AddMinutes(30), EndTime = DateTime.Now.AddDays(-7).Date.AddHours(19).AddMinutes(15) },
            new TrainingSession { UserId = member.Id, StartTime = DateTime.Now.AddDays(-3).Date.AddHours(20), EndTime = DateTime.Now.AddDays(-3).Date.AddHours(21).AddMinutes(5) },
            new TrainingSession { UserId = member.Id, StartTime = DateTime.Now.AddDays(-1).Date.AddHours(8), EndTime = DateTime.Now.AddDays(-1).Date.AddHours(8).AddMinutes(45) }
        };
        context.TrainingSessions.AddRange(sessions);
        context.SaveChanges();

        var types = context.ExerciseTypes.ToList();
        var rnd = new Random();
        foreach (var s in sessions)
        {
            var type = types[rnd.Next(types.Count)];
            context.ExercisePerformeds.Add(new ExercisePerformed
            {
                TrainingSessionId = s.Id,
                ExerciseTypeId = type.Id,
                Load = rnd.Next(10, 100),
                Sets = rnd.Next(3, 5),
                Repetitions = rnd.Next(8, 15)
            });
        }
        context.SaveChanges();
    }
}

app.Run();
