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
    public class GameReportsController : Controller
    {
        private readonly CVGSContext _context;
        public GameReportsController(CVGSContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var game = await _context.Game.Include(a=>a.Category).Include(a=>a.Platform).OrderBy(p => p.Name).ToListAsync();
            return View(game);
        }


       // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
       // {
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
