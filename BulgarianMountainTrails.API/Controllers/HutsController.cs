using AutoMapper;
using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HutsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HutsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /api/huts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HutDto>>> GetHuts([FromQuery] HutFilter filter)
        {
            var allowedKeys = new[] { "minAltitude", "maxAltitude", "mountain", "minCapacity", "maxCapacity" };
            var queryKeys = HttpContext.Request.Query.Keys;

            var invalidKeys = queryKeys.Except(allowedKeys, StringComparer.OrdinalIgnoreCase);
            if (invalidKeys.Any())
            {
                return BadRequest($"Invalid query parameters: {string.Join(", ", invalidKeys)}");
            }

            var query = FilterHuts(filter.MinAltitude, filter.MaxAltitude, filter.Mountain, filter.MinCapacity, filter.MaxCapacity);

            var huts = await query
                .Include(h => h.TrailHuts)
                .ThenInclude(th => th.Trail)
                .Select(h => _mapper.Map<HutDto>(h))
                .ToListAsync();

            if (huts.Count == 0)
                return NotFound("No trails found matching the criteria.");

            return huts;
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

        private IQueryable<Hut> FilterHuts(int? minAltitude, int? maxAltitude, string? mountain, int? minCapacity, int? maxCapacity)
        {
            var query = _context.Huts.AsQueryable();

            if (minAltitude.HasValue)
                query = query.Where(h => h.Altitude >= minAltitude.Value);

            if (maxAltitude.HasValue)
                query = query.Where(h => h.Altitude <= maxAltitude.Value);

            if (!string.IsNullOrEmpty(mountain))
                query = query.Where(h => h.Mountain.ToLower() == mountain.ToLower());

            if (minCapacity.HasValue)
                query = query.Where(h => h.Capacity >= minCapacity.Value);

            if (maxCapacity.HasValue)
                query = query.Where(h => h.Capacity <= maxCapacity.Value);

            return query;
        }

        public class HutFilter
        {
            public string? Mountain { get; set; }
            public int? MinAltitude { get; set; }
            public int? MaxAltitude { get; set; }
            public int? MinCapacity { get; set; }
            public int? MaxCapacity { get; set; }
        }
    }
}
