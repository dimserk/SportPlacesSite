using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPlaces.Models;

namespace SportPlaces.Controllers
{
    //[Authorize]
    public class DBPageController : Controller
    {
        private readonly EntitiesContext _context;

        public DBPageController(EntitiesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.CitiesCount = _context.Cities.Count();
            ViewBag.PhotosCount = _context.Photos.Count();
            ViewBag.SportKindsCount = _context.SportKinds.Count();
            ViewBag.SportObjectsCount = _context.SportObjects.Count();
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.RecordsCount = _context.Records.Count();
                 
            return View();
        }
    }
}