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
        private class TimeItem
        {
            public string ShowTime { get; set; }
            public DateTime Time { get; set; }
        }

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

        public IActionResult AjaxCreate()
        {
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Login");
            ViewBag.SportObjectId = new SelectList(_context.SportObjects, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AjaxCreate(ExpandedRecord expRecord)
        {
            if (ModelState.IsValid)
            {
                var record = new Record();

                record.UserId = expRecord.UserId;
                record.SportObjectId = expRecord.SportObjectId;

                var sportObject = await _context.SportObjects.FindAsync(expRecord.SportObjectId);
                record.Length = expRecord.Length * sportObject.Interval;

                record.Date = expRecord.Date;
                record.Date = record.Date.AddHours(expRecord.Time.Hour);
                record.Date = record.Date.AddMinutes(expRecord.Time.Minute);

                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserId = new SelectList(_context.Users, "Id", "Login");
            ViewBag.SportObjectId = new SelectList(_context.SportObjects, "Id", "Name");
            return View(expRecord);
        }

        // GET: Records/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records.FindAsync(id);
            var user = await _context.Users.FindAsync(record.UserId);
            var sportObject = await _context.SportObjects.FindAsync(record.SportObjectId);

            if (record == null)
            {
                return NotFound();
            }

            var editRecord = new EditRecord();

            editRecord.Id = id ?? default(int);
            EditRecord.SportObjectId = sportObject.Id;
            EditRecord.UserId = user.Id;

            ViewBag.UserName = user.Login;
            ViewBag.SportObjectName = sportObject.Name;

            editRecord.Date = record.Date.Date;
            editRecord.Time = sportObject.Beginning.Date + record.Date.TimeOfDay;

            List<TimeItem> dateTimes = new List<TimeItem>();
            var tmpTime = sportObject.Beginning;
            do
            {
                dateTimes.Add(new TimeItem
                {
                    ShowTime = tmpTime.ToShortTimeString(),
                    Time = tmpTime
                });
                tmpTime = tmpTime.AddHours(sportObject.Interval);
            } while (tmpTime != sportObject.Ending);

            ViewBag.DateTimes = new SelectList(dateTimes, "Time", "ShowTime");

            editRecord.Length = (int)(record.Length / sportObject.Interval);

            List<intervalItem> IntervalList;
            switch (sportObject.Interval)
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

            return View(editRecord);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRecord editRecord)
        {
            var record = new Record();

            record.Id = editRecord.Id;
            record.SportObjectId = EditRecord.SportObjectId;
            record.UserId = EditRecord.UserId;
            record.Date = editRecord.Date;
            record.Date = record.Date + editRecord.Time.TimeOfDay;

            var sportObj = await _context.SportObjects.FindAsync(record.SportObjectId);
            record.Length = editRecord.Length * sportObj.Interval;

            _context.Update(record);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

            ViewBag.Date = record.Date.ToShortDateString();
            ViewBag.Time = record.Date.ToShortTimeString();

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
