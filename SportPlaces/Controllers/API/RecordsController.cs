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
    }
}