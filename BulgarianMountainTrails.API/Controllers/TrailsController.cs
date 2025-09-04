using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ITrailService _service;

        public TrailsController(ITrailService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: /api/trails?minHours=&maxHours=&difficulty=&mountain=&minKm=&maxKm=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrailDto>>> GetTrails
            ([FromQuery] double? minHours, double? maxHours, double? minKm, double? maxKm, string? mountain, string? difficulty)
        {
            var allowedKeys = new[] { "minHours", "maxHours", "difficulty", "mountain", "minKm", "maxKm" };
            var queryKeys = HttpContext.Request.Query.Keys;

            var invalidKeys = queryKeys.Except(allowedKeys, StringComparer.OrdinalIgnoreCase);
            if (invalidKeys.Any())
            {
                return BadRequest($"Invalid query parameters: {string.Join(", ", invalidKeys)}");
            }

            try
            {
                var trails = await _service.GetAllAsync(minHours, maxHours, minKm, maxKm, difficulty, mountain);

                if (!trails.Any())
                    return NotFound("No trails found matching the criteria.");

                return Ok(trails);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: /api/trails/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Trail>> GetTrail(Guid id)
        {
            var trail = await _service.GetByIdAsync(id);

            if (trail == null)
                return NotFound("There is not a Trail with this Id!");

            return Ok(trail);
        }

        // POST: /api/trails/{body}
        [HttpPost]
        public async Task<ActionResult<TrailDto>> PostTrail(TrailDto trail)
        {
            try
            {
                await _service.CreateAsync(trail);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }


            return CreatedAtAction(nameof(GetTrail), new { id = trail.Id }, trail);
        }

        // DELETE: /api/trails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrail(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Successfully deleted!");
        }
    }
}
