using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
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
                return BadRequest($"Invalid query parameters: {string.Join(", ", invalidKeys)}");
            }

            try
            {
                var huts = await _service.GetAllAsync(minAltitude, maxAltitude, minCapacity, maxCapacity, mountain);

                if (!huts.Any())
                    return NotFound("No huts found matching the criteria.");

                return Ok(huts);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: /api/huts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Hut>> GetHut(Guid id)
        {
            var hut = await _service.GetByIdAsync(id);

            if (hut == null)
                return NotFound("There is not a Hut with this Id!");

            return Ok(hut);
        }

        // POST: /api/huts{body}
        [HttpPost]
        public async Task<ActionResult<HutDto>> PostHut(HutDto hut)
        {
            try
            {
                await _service.CreateAsync(hut);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetHut), new { id = hut.Id }, hut);
        }

        // DELETE: /api/huts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHut(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok("Successfully deleted!");
        }
    }
}
