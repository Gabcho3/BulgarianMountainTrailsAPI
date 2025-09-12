using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : Controller
    {
        private readonly ITrailService _service;

        public TrailsController(ITrailService service)
        {
            _service = service;
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
                throw new ArgumentException($"Invalid query parameters: {string.Join(", ", invalidKeys)}");
            }

            var trails = await _service.GetAllAsync(minHours, maxHours, minKm, maxKm, difficulty, mountain);
            return Ok(trails);
        }

        // GET: /api/trails/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Trail>> GetTrail(Guid id)
        {
            var trail = await _service.GetByIdAsync(id);
            return Ok(trail);
        }

        // POST: /api/trails/{body}
        [HttpPost]
        public async Task<ActionResult<TrailDto>> PostTrail(TrailDto trail)
        {
            await _service.CreateAsync(trail);
            return CreatedAtAction(nameof(GetTrail), new { id = trail.Id }, trail);
        }

        // DELETE: /api/trails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrail(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok("Successfully deleted!");
        }
    }
}
