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
    public class SportKindsController : ControllerBase
    {
        private readonly EntitiesContext _context;

        public SportKindsController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: api/SportKinds
        [HttpGet]
        public IEnumerable<SportKind> GetSportKinds()
        {
            return _context.SportKinds;
        }

        // GET: api/SportKinds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSportKind([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sportKind = await _context.SportKinds.FindAsync(id);

            if (sportKind == null)
            {
                return NotFound();
            }

            return Ok(sportKind);
        }

        // PUT: api/SportKinds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSportKind([FromRoute] int id, [FromBody] SportKind sportKind)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sportKind.Id)
            {
                return BadRequest();
            }

            _context.Entry(sportKind).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SportKindExists(id))
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

        // POST: api/SportKinds
        [HttpPost]
        public async Task<IActionResult> PostSportKind([FromBody] SportKind sportKind)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SportKinds.Add(sportKind);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSportKind", new { id = sportKind.Id }, sportKind);
        }

        // DELETE: api/SportKinds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSportKind([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sportKind = await _context.SportKinds.FindAsync(id);
            if (sportKind == null)
            {
                return NotFound();
            }

            _context.SportKinds.Remove(sportKind);
            await _context.SaveChangesAsync();

            return Ok(sportKind);
        }

        private bool SportKindExists(int id)
        {
            return _context.SportKinds.Any(e => e.Id == id);
        }
    }
}