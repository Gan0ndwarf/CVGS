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
    public class ManageEventsController : Controller
    {
        private readonly CVGSContext _context;

        //Constructor for SKTreatmentFertilizerController, initializes OECConext
        public ManageEventsController(CVGSContext context)
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

            var events = _context.Event;

            return View(await events.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Name,Date,Time,Description")]Event events)
        {

            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(events);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event.SingleOrDefaultAsync(m => m.EventId == id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Date,Time,Description")]Event events)
        {

            if (id != events.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(events.EventId))
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
            return View(events);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Event
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (game == null)
            {
                return NotFound();
            }
            // TempData["whichgame"] = id;
            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var game = await _context.Event.SingleOrDefaultAsync(m => m.EventId == id);
            _context.Event.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool EventExists(int? id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
