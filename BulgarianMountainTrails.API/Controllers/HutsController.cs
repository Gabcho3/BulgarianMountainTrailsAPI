using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/huts")]
    [ApiController]
    public class HutsController : Controller
    {
        private readonly IHutService _service;

        public HutsController(IHutService service)
        {
            _service = service;
        }

        // GET: /api/huts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HutDto>>> GetHuts
            ([FromQuery] int? minAltitude, int? maxAltitude, int? minCapacity, int? maxCapacity, string? mountain)
        {
            var allowedKeys = new[] { "minAltitude", "maxAltitude", "mountain", "minCapacity", "maxCapacity" };
            var queryKeys = HttpContext.Request.Query.Keys;

            var invalidKeys = queryKeys.Except(allowedKeys, StringComparer.OrdinalIgnoreCase);
            if (invalidKeys.Any())
            {
                throw new ArgumentException($"Invalid query parameters: {string.Join(", ", invalidKeys)}");
            }

            var huts = await _service.GetAllAsync(minAltitude, maxAltitude, minCapacity, maxCapacity, mountain);
            return Ok(huts);
            
        }

        // GET: /api/huts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Hut>> GetHut(Guid id)
        {
            var hut = await _service.GetByIdAsync(id);
            return Ok(hut);
        }

        // POST: /api/huts{body}
        [HttpPost]
        public async Task<ActionResult<HutDto>> PostHut(HutDto hut)
        {
            await _service.CreateAsync(hut);
            return CreatedAtAction(nameof(GetHut), new { id = hut.Id }, hut);
        }

        // DELETE: /api/huts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHut(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok("Successfully deleted!");
        }
    }
}
