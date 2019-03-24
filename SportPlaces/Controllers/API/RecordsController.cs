using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportPlaces.Models;

namespace SportPlaces.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly EntitiesContext _context;

        public RecordsController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: api/Records
        [HttpGet]
        public IEnumerable<Record> GetRecords()
        {
            return _context.Records;
        }

        // GET: api/Records/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = await _context.Records.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        // GET: api/Records/s/5 SportObjectId
        // GET: api/Records/u/5 UserId
        [HttpGet("{type}/{id}")]
        public IEnumerable<Record> GetRecord([FromRoute] char type, [FromRoute] int id)
        {
            IQueryable<Record> records;

            switch (type)
            {
                case 's':
                    records = from rec in _context.Records where rec.SportObjectId == id select rec;
                    break;
                case 'u':
                    records = from rec in _context.Records where rec.UserId == id select rec;
                    break;
                default:
                    records = null;
                    break;
            }

            return records;
        }

        // PUT: api/Records/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecord([FromRoute] int id, [FromBody] Record record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != record.Id)
            {
                return BadRequest();
            }

            _context.Entry(record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Records
        [HttpPost]
        public async Task<IActionResult> PostRecord([FromBody] Record record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Records.Add(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecord", new { id = record.Id }, record);
        }

        // DELETE: api/Records/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = await _context.Records.FindAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();

            return Ok(record);
        }

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }

        //AjaxApi

        public class TimeItem
        {
            public string ShowTime { get; set; }
            public DateTime Time { get; set; }
        }

        //GET: api/Records/ajax/t/5
        [HttpGet("ajax/t/{id}")]
        public List<TimeItem> GetTime([FromRoute] int id)
        {
            var sportObject = (from so in _context.SportObjects where so.Id == id select so).FirstOrDefault();

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

            return dateTimes;
        }

        public class intervalItem
        {
            public int Length { get; set; }
            public string Name { get; set; }
        }

        //GET: api/Records/ajax/l/5
        [HttpGet("ajax/l/{id}")]
        public List<intervalItem> GetLength([FromRoute] int id)
        {
            var sportObject = (from so in _context.SportObjects where so.Id == id select so).FirstOrDefault();

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

            return IntervalList;
        }

        /// ///////////////////////////////////////

        public class SportObjectRecord
        {
            public string Time { get; set; }
            public string Login { get; set; }
            public string Length { get; set; }
        }

        //GET: api/Records/ajax/s//1/2
        [HttpGet("ajax/s/{Date}/{SportObjectId}")]
        public List<SportObjectRecord> GetSportObjectRecords([FromRoute] string Date, [FromRoute] int SportObjectId)
        {
            var sRecords = new List<SportObjectRecord>();

            DateTime date = new DateTime();
            DateTime.TryParse(Date, out date);
            DateTime dateEnd = date.AddDays(1);

            var allRecords = _context.Records.Include(r => r.User);

            var records = (from rec in allRecords where rec.SportObjectId == SportObjectId && (date <= rec.Date && rec.Date <= dateEnd) select rec).ToList();

            foreach (var rec in records)
            {
                var sRecord = new SportObjectRecord();
                sRecord.Login = rec.User.Login;
                sRecord.Time = rec.Date.ToShortTimeString();
                switch (rec.Length)
                {
                    case 0.5:
                        sRecord.Length = "Тридцать минут";
                        break;
                    case 1:
                        sRecord.Length = "Один час";
                        break;
                    case 1.5:
                        sRecord.Length = "Полтора часа";
                        break;
                    case 2:
                        sRecord.Length = "Два часа";
                        break;
                    case 3:
                        sRecord.Length = "Три часа";
                        break;
                    case 4.5:
                        sRecord.Length = "Четыре с половиной";
                        break;
                }

                sRecords.Add(sRecord);
            }

            return sRecords;
        }

        public class UserRecord
        {
            public string Date { get; set; }
            public string UserName { get; set; }
            public string Length { get; set; }
        }

        //GET: api/Records/ajax/u//1/2
        [HttpGet("ajax/u/{UserId}/{Date}")]
        public List<UserRecord> GetUserRecords([FromRoute] int UserId, [FromRoute] string Date)
        {
            var userRecords = new List<UserRecord>();

            if (Date != "undefined")
            {
                var records = _context.Records.Include(r => r.SportObject).Include(r => r.User);

                DateTime date = new DateTime();
                DateTime.TryParse(Date, out date);
                DateTime dateEnd = date.AddDays(1);

                var fullUserRecords = (from record in records where record.UserId == UserId && (date <= record.Date && record.Date <= dateEnd) select record).ToList();

                foreach (var record in fullUserRecords)
                {
                    var userRec = new UserRecord();
                    userRec.Date = record.Date.ToString();
                    userRec.UserName = record.User.Login;

                    switch (record.Length)
                    {
                        case 0.5:
                            userRec.Length = "Тридцать минут";
                            break;
                        case 1:
                            userRec.Length = "Один час";
                            break;
                        case 1.5:
                            userRec.Length = "Полтора часа";
                            break;
                        case 2:
                            userRec.Length = "Два часа";
                            break;
                        case 3:
                            userRec.Length = "Три часа";
                            break;
                        case 4.5:
                            userRec.Length = "Четыре с половиной";
                            break;
                    }

                    userRecords.Add(userRec);
                }
            }
            else
            {
                var records = _context.Records.Include(r => r.SportObject).Include(r => r.User);

                var fullUserRecords = (from record in records where record.UserId == UserId select record).ToList();

                foreach (var record in fullUserRecords)
                {
                    var userRec = new UserRecord();
                    userRec.Date = record.Date.ToString();
                    userRec.UserName = record.User.Login;

                    switch (record.Length)
                    {
                        case 0.5:
                            userRec.Length = "Тридцать минут";
                            break;
                        case 1:
                            userRec.Length = "Один час";
                            break;
                        case 1.5:
                            userRec.Length = "Полтора часа";
                            break;
                        case 2:
                            userRec.Length = "Два часа";
                            break;
                        case 3:
                            userRec.Length = "Три часа";
                            break;
                        case 4.5:
                            userRec.Length = "Четыре с половиной";
                            break;
                    }

                    userRecords.Add(userRec);
                }
            }
            return userRecords;
        }
    }
}