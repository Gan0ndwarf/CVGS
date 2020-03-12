using CVGS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers
{
    public class AdminPortalController : Controller
    {
        private readonly CVGSContext _context;

        public AdminPortalController(CVGSContext context)
        {
            _context = context;
        }

        public IActionResult Index(string isEmp)
        {
            if (isEmp != null)
            {
                HttpContext.Session.SetString(nameof(isEmp), isEmp);
            }
            else if (HttpContext.Session.GetString(nameof(isEmp)) != null)
            {
                isEmp = HttpContext.Session.GetString(nameof(isEmp));
            }
            else
            {
                TempData["message"] = "Only employees have access to the Admin Portal.";
                return Redirect("/Home/Index");
            }

            return View();
        }

    }
}
