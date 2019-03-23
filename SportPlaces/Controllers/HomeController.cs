using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportPlaces.Models;

namespace SportPlaces.Controllers
{
    public class HomeController : Controller
    {
        private readonly EntitiesContext _context;

        public HomeController(EntitiesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SportObjectsCount = _context.SportObjects.Count();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
