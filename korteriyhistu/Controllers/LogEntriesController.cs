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
    [Route("api/LogEntries")]
    public class LogEntriesController : Controller
    {
        private readonly LogEntriesContext _context;

        public LogEntriesController(LogEntriesContext context)
        {
            _context = context;
        }

        // GET: api/LogEntries
        [HttpGet]
        public IEnumerable<LogEntry> GetLogEntry()
        {
            return _context.LogEntry;
        }

        // GET: api/LogEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogEntry([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logEntry = await _context.LogEntry.SingleOrDefaultAsync(m => m.LogEntryId == id);

            if (logEntry == null)
            {
                return NotFound();
            }

            return Ok(logEntry);
        }

        // PUT: api/LogEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogEntry([FromRoute] string id, [FromBody] LogEntry logEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logEntry.LogEntryId)
            {
                return BadRequest();
            }

            _context.Entry(logEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogEntryExists(id))
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

        // POST: api/LogEntries
        [HttpPost]
        public async Task<IActionResult> PostLogEntry([FromBody] LogEntry logEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.LogEntry.Add(logEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogEntry", new { id = logEntry.LogEntryId }, logEntry);
        }

        // DELETE: api/LogEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogEntry([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logEntry = await _context.LogEntry.SingleOrDefaultAsync(m => m.LogEntryId == id);
            if (logEntry == null)
            {
                return NotFound();
            }

            _context.LogEntry.Remove(logEntry);
            await _context.SaveChangesAsync();

            return Ok(logEntry);
        }

        private bool LogEntryExists(string id)
        {
            return _context.LogEntry.Any(e => e.LogEntryId == id);
        }
    }
}