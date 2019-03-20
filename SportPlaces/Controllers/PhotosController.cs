using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportPlaces.Models;

namespace SportPlaces.Controllers
{
    public class PhotosController : Controller
    {
        private readonly EntitiesContext _context;

        public PhotosController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: Photos
        public async Task<IActionResult> Index(int? selectedId)
        {
            var entitiesContext = _context.Photos.Include(p => p.SportObject);

            var sportObjects = _context.SportObjects;
            ViewBag.SportObjects = new SelectList(sportObjects, "Id", "Name");

            if (selectedId != null)
            {
                var plist = from p in entitiesContext where p.SportObject.Id == selectedId select p;
                return View(await plist.ToListAsync());
            }

            return View(await entitiesContext.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.SportObject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SportObjectId,Image")] Photo photo, List<IFormFile> Image)
        {
            foreach (var item in Image)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        photo.Image = stream.ToArray();
                    }
                }
            }

            _context.Add(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name", photo.SportObjectId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SportObjectId,Image")] Photo photo, List<IFormFile> Image)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            try
            {
                foreach (var item in Image)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            photo.Image = stream.ToArray();
                        }
                    }
                }

                _context.Update(photo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(photo.Id))
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

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.SportObject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}
