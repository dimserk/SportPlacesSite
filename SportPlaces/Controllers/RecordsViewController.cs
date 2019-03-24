using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportPlaces.Models;

namespace SportPlaces.Controllers
{
    public class RecordsViewController : Controller
    {
        private readonly EntitiesContext _context;

        public RecordsViewController(EntitiesContext context)
        {
            _context = context;
        }

        public IActionResult UsersRecords()
        {
            ViewBag.UsersSelect = new SelectList(_context.Users, "Id", "Login");

            return View();
        }

        public IActionResult SportObjectsRecords()
        {
            ViewBag.SportObjectsSelect = new SelectList(_context.SportObjects, "Id", "Name");

            return View();
        }
    }
}