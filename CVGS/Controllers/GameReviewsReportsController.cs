using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;

namespace CVGS.Controllers
{
    public class GameReviewsReportsController : Controller
    {
        private readonly CVGSContext _context;
        public GameReviewsReportsController(CVGSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var review = await _context.Review.Include(a => a.User).Include(a => a.Game).OrderBy(p => p.GameId).ToListAsync();
            return View(review);
        }


       // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
       // {
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
