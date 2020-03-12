using CVGS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers
{
    public class ManageReviewsController : Controller
    {
        private readonly CVGSContext _context;

        public ManageReviewsController(CVGSContext context)
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

            var reviews = _context.Review
                          .Include(a => a.Game)
                          .Include(a => a.User)
                          .Where(a => a.ApprovedFlag == false)
                          .ToListAsync();

            return View(await reviews);
        }



        public async Task<IActionResult> ApproveReview(int? userId, int? gameId)
        {
            if (userId == null || gameId == null)
            {
                return NotFound();
            }

            var review = await _context.Review.SingleOrDefaultAsync(m => m.GameId == gameId && m.UserId == userId);
            if (review == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    review.ApprovedFlag = true;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.GameId, review.UserId))
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

            return View(review);
        }


        public async Task<IActionResult> RejectReview(int? userId, int? gameId)
        {
            if (userId == null || gameId == null)
            {
                return NotFound();
            }

            var review = await _context.Review.SingleOrDefaultAsync(m => m.GameId == gameId && m.UserId == userId);
            if (review == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Review.Remove(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.GameId, review.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }




        private bool ReviewExists(int? gameId, int? userId)
        {
            return _context.Review.Any(e => e.GameId == gameId && e.UserId == userId);
        }

    }
}
