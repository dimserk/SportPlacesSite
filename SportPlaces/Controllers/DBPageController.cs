using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SportPlaces.Controllers
{
    public class DBPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}