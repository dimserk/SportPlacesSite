using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportPlaces.Models;

namespace SportPlaces.Controllers
{
    public class SportObjectsController : Controller
    {
        private readonly EntitiesContext _context;

        public SportObjectsController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: SportObjects
        public async Task<IActionResult> Index()
        {
            var entitiesContext = _context.SportObjects.Include(s => s.City).Include(s => s.SportKind);
            return View(await entitiesContext.ToListAsync());
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

            return View(sportObject);
        }

        // GET: SportObjects/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            ViewData["SportKindId"] = new SelectList(_context.SportKinds, "Id", "SportKindName");
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
                _context.Add(sportObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", sportObject.CityId);
            ViewData["SportKindId"] = new SelectList(_context.SportKinds, "Id", "SportKindName", sportObject.SportKindId);
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
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportObjectExists(sportObject.Id))
                    {
                        return NotFound();
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
