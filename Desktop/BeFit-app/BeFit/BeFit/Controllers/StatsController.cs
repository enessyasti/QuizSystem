using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var exercises = await _context.ExercisePerformeds
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .Where(e => e.TrainingSession.UserId == user.Id)
                .ToListAsync();

            var stats = exercises
                .GroupBy(e => e.ExerciseType.Name)
                .Select(g => new ExerciseStatisticsViewModel
                {
                    ExerciseName = g.Key,
                    Frequency = g.Count(e => e.TrainingSession.StartTime >= fourWeeksAgo),
                    TotalRepetitions = g.Sum(e => e.Sets * e.Repetitions),
                    AverageLoad = g.Average(e => e.Load),
                    MaxLoad = g.Max(e => e.Load)
                })
                .ToList();

            return View(stats);
        }
    }
}
