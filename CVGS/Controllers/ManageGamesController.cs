using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;
using CVGS.Models.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace CVGS.Controllers
{
    public class ManageGamesController : Controller
    {
        private readonly CVGSContext _context;

        //Constructor for SKTreatmentFertilizerController, initializes OECConext
        public ManageGamesController(CVGSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));

            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var games = await _context.Game
                          .Include(a => a.Category)
                          .Include(a => a.Platform)
                          .ToListAsync();

            //Set overall ratings
            foreach (Game game in games)
            {
                var reviews = _context.Review
                .Include(a => a.Game)
                .Where(a => a.GameId == game.GameId && a.ApprovedFlag == true)
                .OrderBy(a => a.Date)
                .ToList();

                decimal? overallRating = null;

                if (reviews.Count() > 0)
                {
                    overallRating = (int)reviews.Sum(a => a.Rating) / (decimal)reviews.Count();
                }

                game.Rating = overallRating;
            }

            return View(games);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(a => a.Platform)
                .Include(a => a.Category)
                .SingleOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        public IActionResult Create()
        {
            ViewData["PlatformList"] = new SelectList(_context.LookupPlatform.OrderBy(p => p.Platform), "Id", "Platform");
            ViewData["CategoryList"] = new SelectList(_context.LookupCategory.OrderBy(p => p.Category), "Id", "Category");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,CategoryId,PlatformId,Name,ReleaseDate,Description,Rating,Price,Developer,ImgLocation")]Game game)
        {

            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PlatformList"] = new SelectList(_context.LookupPlatform.OrderBy(p => p.Platform), "Id", "Platform", game.PlatformId);
            ViewData["CategoryList"] = new SelectList(_context.LookupCategory.OrderBy(p => p.Category), "Id", "Category", game.CategoryId);

            return View(game);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.SingleOrDefaultAsync(a => a.GameId == id);

            ViewData["PlatformList"] = new SelectList(_context.LookupPlatform.OrderBy(p => p.Platform), "Id", "Platform", game.PlatformId);
            ViewData["CategoryList"] = new SelectList(_context.LookupCategory.OrderBy(p => p.Category), "Id", "Category", game.CategoryId);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,CategoryId,PlatformId,Name,ReleaseDate,Description,Rating,Price,Developer,ImgLocation")]Game game)
        {

            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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

            ViewData["PlatformList"] = new SelectList(_context.LookupPlatform.OrderBy(p => p.Platform), "Id", "Platform", game.PlatformId);
            ViewData["CategoryList"] = new SelectList(_context.LookupCategory.OrderBy(p => p.Category), "Id", "Category", game.CategoryId);

            return View(game);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(a => a.Platform)
                .Include(a => a.Category)
                .SingleOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var game = await _context.Game.SingleOrDefaultAsync(m => m.GameId == id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool GameExists(int? id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
