using AutoMapper;
using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using BulgarianMountainTrails.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrailsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /api/trails?minHours=&maxHours=&difficulty=&mountain=&minKm=&maxKm=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrailDto>>> GetTrails([FromQuery] TrailFilter filter)
        {
            var allowedKeys = new[] { "minHours", "maxHours", "difficulty", "mountain", "minKm", "maxKm" };
            var queryKeys = HttpContext.Request.Query.Keys;

            var invalidKeys = queryKeys.Except(allowedKeys, StringComparer.OrdinalIgnoreCase);
            if (invalidKeys.Any())
            {
                return BadRequest($"Invalid query parameters: {string.Join(", ", invalidKeys)}");
            }

            var query = FilterTrails(filter.MinHours, filter.MaxHours, filter.Mountain, filter.MinKm, filter.MaxKm);

            if (filter.Difficulty != null)
            {
                bool isValidDifficulty = Enum.TryParse<DifficultyEnum>(filter.Difficulty, true, out var difficultyEnum);

                if (!isValidDifficulty)
                    return BadRequest("Invalid difficulty level.");

                query = query.Where(t => t.Difficulty == difficultyEnum);
            }

            var trails =  await query
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .Select(t => _mapper.Map<TrailDto>(t))
                .ToListAsync();

            if (trails.Count == 0)
                return NotFound("No trails found matching the criteria.");

            return trails;
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

        private IQueryable<Trail> FilterTrails(double? minHours, double? maxHours, string? mountain, double? minKm, double? maxKm)
        {
            var query = _context.Trails.AsQueryable();

            if (minHours.HasValue)
                query = query.Where(t => t.DurationHours >= minHours.Value);

            if (maxHours.HasValue)
                query = query.Where(t => t.DurationHours <= maxHours.Value);

            if (mountain != null)
                query = query.Where(t => t.Mountain.ToLower() == mountain!.ToLower());

            if (minKm.HasValue)
                query = query.Where(t => t.LengthKm >= minKm.Value);

            if (maxKm.HasValue)
                query = query.Where(t => t.LengthKm <= maxKm.Value);

            return query;
        }

        public class TrailFilter
        {
            public double? MinHours { get; set; }
            public double? MaxHours { get; set; }
            public string? Difficulty { get; set; }
            public string? Mountain { get; set; }
            public double? MinKm { get; set; }
            public double? MaxKm { get; set; }
        }
    }
}
