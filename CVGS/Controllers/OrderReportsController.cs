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
    public class OrderReportsController : Controller
    {
        private readonly CVGSContext _context;

        public OrderReportsController(CVGSContext context)
        {
            _context = context;
        }

        // GET: OrderReports
        public async Task<IActionResult> Index()
        {
            var orders = _context.Order.Include(o => o.User).Include(o => o.Game);
            return View(await orders.ToListAsync());
        }
    }
}
