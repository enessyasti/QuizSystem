using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Controllers
{
    [Authorize]
    public class ExercisePerformedsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExercisePerformedsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var applicationDbContext = _context.ExercisePerformeds
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .Where(e => e.TrainingSession.UserId == user.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var exercisePerformed = await _context.ExercisePerformeds
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id && m.TrainingSession.UserId == user.Id);

            if (exercisePerformed == null)
            {
                return NotFound();
            }

            return View(exercisePerformed);
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            
            var userSessions = _context.TrainingSessions
                .Where(t => t.UserId == user.Id)
                .Select(s => new { Id = s.Id, DisplayText = s.StartTime.ToString("g") });

            ViewData["TrainingSessionId"] = new SelectList(userSessions, "Id", "DisplayText");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingSessionId,ExerciseTypeId,Load,Sets,Repetitions")] ExercisePerformedCreateDto exercisePerformedDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var session = await _context.TrainingSessions.FirstOrDefaultAsync(s => s.Id == exercisePerformedDto.TrainingSessionId && s.UserId == user.Id);
            if (session == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var exercisePerformed = new ExercisePerformed
                {
                    TrainingSessionId = exercisePerformedDto.TrainingSessionId,
                    ExerciseTypeId = exercisePerformedDto.ExerciseTypeId,
                    Load = exercisePerformedDto.Load,
                    Sets = exercisePerformedDto.Sets,
                    Repetitions = exercisePerformedDto.Repetitions
                };

                _context.Add(exercisePerformed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exercisePerformedDto.ExerciseTypeId);
            
             var userSessions = _context.TrainingSessions
                .Where(t => t.UserId == user.Id)
                .Select(s => new { Id = s.Id, DisplayText = s.StartTime.ToString("g") });
            ViewData["TrainingSessionId"] = new SelectList(userSessions, "Id", "DisplayText", exercisePerformedDto.TrainingSessionId);
            return View(exercisePerformedDto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var exercisePerformed = await _context.ExercisePerformeds
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(e => e.Id == id && e.TrainingSession.UserId == user.Id);

            if (exercisePerformed == null)
            {
                return NotFound();
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exercisePerformed.ExerciseTypeId);
            
            var userSessions = _context.TrainingSessions
                .Where(t => t.UserId == user.Id)
                .Select(s => new { Id = s.Id, DisplayText = s.StartTime.ToString("g") });
            ViewData["TrainingSessionId"] = new SelectList(userSessions, "Id", "DisplayText", exercisePerformed.TrainingSessionId);
            return View(exercisePerformed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrainingSessionId,ExerciseTypeId,Load,Sets,Repetitions")] ExercisePerformed exercisePerformed)
        {
            if (id != exercisePerformed.Id)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var session = await _context.TrainingSessions.FirstOrDefaultAsync(s => s.Id == exercisePerformed.TrainingSessionId && s.UserId == user.Id);
            if (session == null)
            {
                 return NotFound();
            }
            
             var existingExercise = await _context.ExercisePerformeds.AsNoTracking()
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(e => e.Id == id && e.TrainingSession.UserId == user.Id);
            
            if (existingExercise == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercisePerformed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExercisePerformedExists(exercisePerformed.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exercisePerformed.ExerciseTypeId);
            
            var userSessions = _context.TrainingSessions
                .Where(t => t.UserId == user.Id)
                .Select(s => new { Id = s.Id, DisplayText = s.StartTime.ToString("g") });
            ViewData["TrainingSessionId"] = new SelectList(userSessions, "Id", "DisplayText", exercisePerformed.TrainingSessionId);
            return View(exercisePerformed);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var exercisePerformed = await _context.ExercisePerformeds
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id && m.TrainingSession.UserId == user.Id);

            if (exercisePerformed == null)
            {
                return NotFound();
            }

            return View(exercisePerformed);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var exercisePerformed = await _context.ExercisePerformeds
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(e => e.Id == id && e.TrainingSession.UserId == user.Id);

            if (exercisePerformed != null)
            {
                _context.ExercisePerformeds.Remove(exercisePerformed);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ExercisePerformedExists(int id)
        {
            return _context.ExercisePerformeds.Any(e => e.Id == id);
        }
    }
}
