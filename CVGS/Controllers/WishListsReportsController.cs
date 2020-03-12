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
    public class WishListsReportsController : Controller
    {
        private readonly CVGSContext _context;

        public WishListsReportsController(CVGSContext context)
        {
            _context = context;
        }

        // GET: UserGameWishListsReport
        public async Task<IActionResult> Index()
        {
            var wishListReports = _context.UserGameWishList
                                .Include(u => u.Game)
                                .Include(u => u.User)
                                .GroupBy(u => u.Game)
                                .Select(a => new WishListReportsModel { Game = a.Key, UserCount = a.Count() });

            return View(await wishListReports.ToListAsync());
        }

    }
}
