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
    public class SportObjectsController : ControllerBase
    {
        private readonly EntitiesContext _context;

        public SportObjectsController(EntitiesContext context)
        {
            _context = context;
        }

        // GET: api/SportObjects
        [HttpGet]
        public IEnumerable<SportObject> GetSportObjects()
        {
            return _context.SportObjects;
        }

        // GET: api/SportObjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSportObject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sportObject = await _context.SportObjects.FindAsync(id);

            if (sportObject == null)
            {
                return NotFound();
            }

            return Ok(sportObject);
        }

        // PUT: api/SportObjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSportObject([FromRoute] int id, [FromBody] SportObject sportObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sportObject.Id)
            {
                return BadRequest();
            }

            _context.Entry(sportObject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SportObjectExists(id))
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

        // POST: api/SportObjects
        [HttpPost]
        public async Task<IActionResult> PostSportObject([FromBody] SportObject sportObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SportObjects.Add(sportObject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSportObject", new { id = sportObject.Id }, sportObject);
        }

        // DELETE: api/SportObjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSportObject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sportObject = await _context.SportObjects.FindAsync(id);
            if (sportObject == null)
            {
                return NotFound();
            }

            _context.SportObjects.Remove(sportObject);
            await _context.SaveChangesAsync();

            return Ok(sportObject);
        }

        private bool SportObjectExists(int id)
        {
            return _context.SportObjects.Any(e => e.Id == id);
        }
    }
}