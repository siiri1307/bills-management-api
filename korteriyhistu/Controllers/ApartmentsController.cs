using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using korteriyhistu.Models;

namespace korteriyhistu.Controllers
{
    [Produces("application/json")]
    [Route("api/Apartments")]
    public class ApartmentsController : Controller
    {
        private readonly ApartmentsContext _context;

        public ApartmentsController(ApartmentsContext context)
        {
            _context = context;
        }

        // GET: api/Apartments
        [HttpGet]
        public IEnumerable<Apartment> GetApartment()
        {
            return _context.Apartment;
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var apartment = await _context.Apartment.SingleOrDefaultAsync(m => m.id == id);

            if (apartment == null)
            {
                return NotFound();
            }

            return Ok(apartment);
        }

        // PUT: api/Apartments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment([FromRoute] int id, [FromBody] Apartment apartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != apartment.id)
            {
                return BadRequest();
            }

            _context.Entry(apartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(id))
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

        // POST: api/Apartments
        [HttpPost]
        public async Task<IActionResult> PostApartment([FromBody] Apartment apartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Apartment.Add(apartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApartment", new { id = apartment.id }, apartment);
        }

        // DELETE: api/Apartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var apartment = await _context.Apartment.SingleOrDefaultAsync(m => m.id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            _context.Apartment.Remove(apartment);
            await _context.SaveChangesAsync();

            return Ok(apartment);
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartment.Any(e => e.id == id);
        }
    }
}