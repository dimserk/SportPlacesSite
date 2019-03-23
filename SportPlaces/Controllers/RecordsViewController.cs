using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult SportObjectsRecords()
        {
            return View();
        }
    }
}