using BulgarianMountainTrailsAPI.Data;
using BulgarianMountainTrailsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrailsController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: /api/trails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trail>>> GetTrails()
        {
            return await _context.Trails
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .ToListAsync();
        }

        // GET: /api/trails/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Trail>> GetTrail(Guid id)
        {
            var trail = await _context.Trails
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trail == null)
                return NotFound();

            return trail;
        }

        // POST: /api/trails/{body}
        [HttpPost]
        public async Task<ActionResult<Trail>> PostTrail(Trail trail)
        {
            await _context.Trails.AddAsync(trail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrail), new { id = trail.Id }, trail);
        }

        // DELETE: /api/trails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrail(Guid id)
        {
            var trail = await _context.Trails.FindAsync(id);

            if (trail == null)
                return NotFound();

            _context.Trails.Remove(trail);
            await _context.SaveChangesAsync();

            return Accepted();
        }
    }
}
