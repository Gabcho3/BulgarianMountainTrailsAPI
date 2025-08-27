using BulgarianMountainTrailsAPI.Data;
using BulgarianMountainTrailsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailHutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrailHutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/trailhut/trail/{id}
        [HttpGet("trail/{trailId}")]
        public async Task<ActionResult> GetHutsForTrail(Guid trailId)
        {
            var huts = await _context.TrailHuts
                .Where(th => th.TrailId == trailId)
                .Select(th => th.Hut)
                .ToListAsync();

            return Ok(huts);
        }

        // GET: /api/trailhut/hut/{id}
        [HttpGet("hut/{hutId}")]
        public async Task<ActionResult> GetTrailsForHut(Guid hutId)
        {
            var trails = await _context.TrailHuts
                .Where(th => th.HutId == hutId)
                .Select(th => th.Trail)
                .ToListAsync();

            return Ok(trails);
        }

        // POST: /api/trailhut/{body}
        [HttpPost]
        public async Task<ActionResult> AddHutToTrail(TrailHut trailHut)
        {
            await _context.TrailHuts.AddAsync(trailHut);
            await _context.SaveChangesAsync();

            return Ok(trailHut);
        }

        // DELETE: /api/trailhut/{body}
        [HttpDelete]
        public async Task<ActionResult> DeleteHutToTrail(TrailHut trailHut)
        {
            _context.TrailHuts.Remove(trailHut);
            await _context.SaveChangesAsync();

            return Ok(trailHut);
        }
    }
}
