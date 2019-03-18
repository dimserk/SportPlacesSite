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
    public class RecordsController : Controller
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

        public RecordsController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: Records
        public async Task<IActionResult> Index()
        {
            var entitiesContext = _context.Records.Include(r => r.SportObject).Include(r => r.User);
            return View(await entitiesContext.ToListAsync());
        }

        // GET: Records/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .Include(r => r.SportObject)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // GET: Records/Create
        public IActionResult Create()
        {
            ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login");
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Date,Length,SportObjectId,UserId")] Record record)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(record);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name", record.SportObjectId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", record.UserId);
        //    return View(record);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreRecord preRecord)
        {
            Record record = new Record(); 

            if (ModelState.IsValid)
            {
                record.UserId = _user.Id;
                record.SportObjectId = _sportObject.Id;

                record.Length = _sportObject.Interval * preRecord.PreLength;

                record.Date = preRecord.Date;
                record.Date = preRecord.Date.AddHours(preRecord.Time.Hour);
                record.Date = record.Date.AddMinutes(preRecord.Time.Minute);

                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name", record.SportObjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", record.UserId);
            return View(record);
        }

        private static SportObject _sportObject;
        private static User _user;

        public IActionResult SubCreate()
        {
            ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login");
            return View();
        }

        private class TimeItem
        {
            public string ShowTime { get; set; }
            public DateTime Time { get; set; }
        }

        public IActionResult FinalCreate(HelpRecord subModel)
        {
            _sportObject = (from sportObj in _context.SportObjects where sportObj.Id == subModel.SportObjectId select sportObj).FirstOrDefault();
            _user = (from user in _context.Users where user.Id == subModel.UserId select user).FirstOrDefault();

            ViewBag.SportObjectName = _sportObject.Name;
            ViewBag.UserName = _user.Login;

            List<TimeItem> dateTimes = new List<TimeItem>();
            var tmpTime = _sportObject.Beginning;
            do
            {
                dateTimes.Add(new TimeItem
                {
                    ShowTime = tmpTime.ToShortTimeString(),
                    Time = tmpTime
                });
                tmpTime = tmpTime.AddHours(_sportObject.Interval);
            } while (tmpTime != _sportObject.Ending);

            ViewBag.DateTimes = new SelectList(dateTimes, "Time", "ShowTime");

            List<intervalItem> IntervalList;
            switch (_sportObject.Interval)
            {
                case 0.5:
                    IntervalList = new List<intervalItem>()
                    {
                        new intervalItem { Name = "Тридцать минут", Length = 1},
                        new intervalItem { Name = "Один час", Length = 2 },
                        new intervalItem { Name = "Полтора часа", Length = 3 }
                    };
                    break;
                case 1:
                    IntervalList = new List<intervalItem>()
                    {
                        new intervalItem { Name = "Один час", Length = 1},
                        new intervalItem { Name = "Два часа", Length = 2 },
                        new intervalItem { Name = "Три часа", Length = 3 }
                    };
                    break;
                case 1.5:
                    IntervalList = new List<intervalItem>()
                    {
                        new intervalItem { Name = "Полтора часа", Length = 1},
                        new intervalItem { Name = "Три часа", Length = 2 },
                        new intervalItem { Name = "Четыре с половиной часа", Length = 3 }
                    };
                    break;
                default:
                    IntervalList = new List<intervalItem>();
                    break;
            }
            ViewBag.RecordIntervals = new SelectList(IntervalList, "Length", "Name");

            return View();
        }

        // GET: Records/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records.FindAsync(id);
            if (record == null)
            {
                return NotFound();
            }
            ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name", record.SportObjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", record.UserId);
            return View(record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Length,SportObjectId,UserId")] Record record)
        {
            if (id != record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(record.Id))
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
            ViewData["SportObjectId"] = new SelectList(_context.SportObjects, "Id", "Name", record.SportObjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", record.UserId);
            return View(record);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .Include(r => r.SportObject)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _context.Records.FindAsync(id);
            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}
