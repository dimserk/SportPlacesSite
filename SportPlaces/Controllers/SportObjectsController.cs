using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportPlaces.Models;

namespace SportPlaces.Controllers
{
    [Authorize]
    public class SportObjectsController : Controller
    {
        private class intervalItem
        {
            public int Length { get; set; }
            public string Name { get; set; }
        }

        private List<intervalItem> intervals = new List<intervalItem>()
        {
            new intervalItem { Length = 1, Name="Тридцать минут"},
            new intervalItem { Length = 2, Name="Один час"},
            new intervalItem { Length = 3, Name="Полтора часа"},
        };

        private void GetIntervalName(double value)
        {
            switch (value)
            {
                case 0.5:
                    ViewBag.IntervalName = "Тридцать минут";
                    break;
                case 1:
                    ViewBag.IntervalName = "Один час ";
                    break;
                case 1.5:
                    ViewBag.IntervalName = "Полтора часа";
                    break;
            }
        }

        private readonly EntitiesContext _context;

        public SportObjectsController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: SportObjects
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StartSortParm"] = sortOrder == "Start" ? "start_desc" : "Start";

            var sportObjects = from s in _context.SportObjects.Include(s => s.City).Include(s => s.SportKind) select s;

            switch (sortOrder)
            {
                case "name_desc":
                    sportObjects = sportObjects.OrderByDescending(s => s.Name);
                    break;
                case "Start":
                    sportObjects = sportObjects.OrderBy(s => s.Beginning);
                    break;
                case "start_desc":
                    sportObjects = sportObjects.OrderByDescending(s => s.Beginning);
                    break;
                default:
                    sportObjects = sportObjects.OrderBy(s => s.Name);
                    break;
            }

            return View(await sportObjects.ToListAsync());

            //var entitiesContext = _context.SportObjects.Include(s => s.City).Include(s => s.SportKind);
            //return View(await entitiesContext.ToListAsync());
        }

        // GET: SportObjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportObject = await _context.SportObjects
                .Include(s => s.City)
                .Include(s => s.SportKind)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportObject == null)
            {
                return NotFound();
            }

            GetIntervalName(sportObject.Interval);

            return View(sportObject);
        }

        // GET: SportObjects/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            ViewData["SportKindId"] = new SelectList(_context.SportKinds, "Id", "SportKindName");
            ViewBag.Intervals = new SelectList(intervals, "Length", "Name");
            return View();
        }

        // POST: SportObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Discription,MaxPeople,Beginning,Ending,Interval,SportKindId,CityId")] SportObject sportObject)
        {
            if (ModelState.IsValid)
            {
                switch(sportObject.Interval)
                {
                    case 1:
                        sportObject.Interval = 0.5;
                        break;
                    case 2:
                        sportObject.Interval = 1;
                        break;
                    case 3:
                        sportObject.Interval = 1.5;
                        break;
                }

                var workLen = sportObject.Beginning.Subtract(sportObject.Ending);
                if (workLen.TotalHours % sportObject.Interval == 0)
                {
                    _context.Add(sportObject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Неверный интервал занятий!";
                }
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", sportObject.CityId);
            ViewData["SportKindId"] = new SelectList(_context.SportKinds, "Id", "SportKindName", sportObject.SportKindId);
            ViewBag.Intervals = new SelectList(intervals, "Length", "Name");
            return View(sportObject);
        }

        // GET: SportObjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportObject = await _context.SportObjects.FindAsync(id);
            if (sportObject == null)
            {
                return NotFound();
            }

            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", sportObject.CityId);
            ViewData["SportKindId"] = new SelectList(_context.SportKinds, "Id", "SportKindName", sportObject.SportKindId);
            ViewBag.Intervals = new SelectList(intervals, "Length", "Name");
            return View(sportObject);
        }

        // POST: SportObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Discription,MaxPeople,Beginning,Ending,Interval,SportKindId,CityId")] SportObject sportObject)
        {
            if (id != sportObject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportObject);
                    switch (sportObject.Interval)
                    {
                        case 1:
                            sportObject.Interval = 0.5;
                            break;
                        case 2:
                            sportObject.Interval = 1;
                            break;
                        case 3:
                            sportObject.Interval = 1.5;
                            break;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportObjectExists(sportObject.Id))
                    {
                        return RedirectToAction(nameof(Error));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", sportObject.CityId);
            ViewData["SportKindId"] = new SelectList(_context.SportKinds, "Id", "SportKindName", sportObject.SportKindId);
            return View(sportObject);
        }

        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        // GET: SportObjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportObject = await _context.SportObjects
                .Include(s => s.City)
                .Include(s => s.SportKind)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportObject == null)
            {
                return NotFound();
            }

            GetIntervalName(sportObject.Interval);

            return View(sportObject);
        }

        // POST: SportObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sportObject = await _context.SportObjects.FindAsync(id);
            _context.SportObjects.Remove(sportObject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportObjectExists(int id)
        {
            return _context.SportObjects.Any(e => e.Id == id);
        }
    }
}
