using BulgarianMountainTrailsAPI.Data;
using BulgarianMountainTrailsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HutsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/huts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hut>>> GetHuts()
        {
            return await _context.Huts
                .Include(h => h.TrailHuts)
                .ThenInclude(th => th.Trail)
                .ToListAsync();
        }

        // GET: /api/huts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Hut>> GetHut(Guid id)
        {
            var hut = await _context.Huts
                .Include(h => h.TrailHuts)
                .ThenInclude(th => th.Trail)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hut == null)
                return NotFound();

            return hut;
        }

        // GET: /api/huts/mountain/{mountain}
        [HttpGet("mountain/{mountain}")]
        public async Task<ActionResult<IEnumerable<Hut>>> GetHutsByMountain(string mountain)
        {
            return await _context.Huts
                .Where(h => h.Mountain.ToLower() == mountain.ToLower())
                .Include(h => h.TrailHuts)
                .ThenInclude(th => th.Trail)
                .ToListAsync();
        }

        // POST: /api/huts{body}
        [HttpPost]
        public async Task<ActionResult<Hut>> PostHut(Hut hut)
        {
            await _context.Huts.AddAsync(hut);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHut), new { id = hut.Id }, hut);
        }

        // DELETE: /api/huts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHut(Guid id)
        {
            var hut = await _context.Huts.FindAsync(id);

            if (hut == null)
                return NotFound();

            _context.Huts.Remove(hut);
            await _context.SaveChangesAsync();

            return Accepted();
        }
    }
}
