using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportPlaces.Models;

namespace SportPlaces.Controllers
{
    [Authorize]
    public class SiteUsersController : Controller
    {
        private readonly EntitiesContext _context;

        public SiteUsersController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: SiteUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.SiteUsers.ToListAsync());
        }

        // GET: SiteUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (User.Identity.Name != "admin")
            {
                return RedirectToAction(nameof(Index));
            }

            var siteUser = await _context.SiteUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siteUser == null)
            {
                return NotFound();
            }

            return View(siteUser);
        }

        // GET: SiteUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SiteUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Password")] SiteUser siteUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(siteUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(siteUser);
        }

        // GET: SiteUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (User.Identity.Name != "admin")
            {
                return RedirectToAction(nameof(Index));
            }

            var siteUser = await _context.SiteUsers.FindAsync(id);
            if (siteUser == null)
            {
                return NotFound();
            }
            return View(siteUser);
        }

        // POST: SiteUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password")] SiteUser siteUser)
        {
            if (id != siteUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(siteUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiteUserExists(siteUser.Id))
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
            return View(siteUser);
        }

        // GET: SiteUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (User.Identity.Name != "admin")
            {
                return RedirectToAction(nameof(Index));
            }

            var siteUser = await _context.SiteUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siteUser == null)
            {
                return NotFound();
            }

            return View(siteUser);
        }

        // POST: SiteUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var siteUser = await _context.SiteUsers.FindAsync(id);
            _context.SiteUsers.Remove(siteUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiteUserExists(int id)
        {
            return _context.SiteUsers.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
