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
    public class SportKindsController : Controller
    {
        private readonly EntitiesContext _context;

        public SportKindsController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: SportKinds
        public async Task<IActionResult> Index()
        {
            return View(await _context.SportKinds.ToListAsync());
        }

        // GET: SportKinds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportKinds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SportKindName")] SportKind sportKind)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sportKind);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sportKind);
        }

        // GET: SportKinds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportKind = await _context.SportKinds.FindAsync(id);
            if (sportKind == null)
            {
                return NotFound();
            }
            return View(sportKind);
        }

        // POST: SportKinds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SportKindName")] SportKind sportKind)
        {
            if (id != sportKind.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportKind);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportKindExists(sportKind.Id))
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
            return View(sportKind);
        }

        // GET: SportKinds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportKind = await _context.SportKinds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportKind == null)
            {
                return NotFound();
            }

            return View(sportKind);
        }

        // POST: SportKinds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sportKind = await _context.SportKinds.FindAsync(id);
            _context.SportKinds.Remove(sportKind);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportKindExists(int id)
        {
            return _context.SportKinds.Any(e => e.Id == id);
        }
    }
}
