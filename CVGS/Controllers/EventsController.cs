using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CVGS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CVGS.Controllers
{
    public class EventsController : Controller
    {
        private readonly CVGSContext _context;

        public EventsController(CVGSContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int? id = null;
            int? userId = null;
            var currentDate = DateTime.Now;

            List<Event> events = _context.Event.Where(a => currentDate <= a.Date).ToList();


            if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                id = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }

            if (id != null)
            {
                List<Event> registeredEvents = _context.UserEvent
                    .Where(a => a.User.UserId == id && currentDate <= a.Event.Date)
                    .Select(a => a.Event)
                    .ToList();

                events = events.Except(registeredEvents)
                            .OrderBy(a => a.Date)
                            .OrderBy(a => a.Time)
                            .ToList();
            }


            return View(events);
        }

        public async Task<IActionResult> Register(int eventId, int userId)
        {
            UserEvent userEvent = new UserEvent();
            userEvent.EventId = eventId;
            userEvent.UserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(userEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(userEvent);
        }

        private bool EventExists(int? eventId)
        {
            return _context.Event.Any(e => e.EventId == eventId);
        }

    }
}
