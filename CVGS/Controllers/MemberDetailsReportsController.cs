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
    public class MemberDetailsReportsController : Controller
    {
        private readonly CVGSContext _context;
        public MemberDetailsReportsController(CVGSContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _context.User.Include(a=>a.Gender).OrderBy(p => p.UserId).ToListAsync();  
            return View(user);
        }

    }
}
