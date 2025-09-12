using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailHutController : ControllerBase
    {
        private readonly ITrailHutService _service;

        public TrailHutController(ITrailHutService service)
        {
            _service = service;
        }

        // GET: /api/trailhut/trail/{id}
        [HttpGet("trail/{trailId}")]
        public async Task<ActionResult> GetHutsForTrail(Guid trailId)
        {
            try
            {
                var huts = await _service.GetHutsForTrailAsync(trailId);

                if (!huts.Any())
                    return NotFound("No Huts found for this Trail!");

                return Ok(huts);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: /api/trailhut/hut/{id}
        [HttpGet("hut/{hutId}")]
        public async Task<ActionResult> GetTrailsForHut(Guid hutId)
        {
            try
            {
                var trails = await _service.GetTrailsForHutAsync(hutId);

                if (!trails.Any())
                    return NotFound("No Trails found for this Hut!");

                return Ok(trails);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: /api/trailhut/{body}
        [HttpPost]
        public async Task<ActionResult> AddHutToTrail(TrailHutDto trailHutDto)
        {
            try
            {
                await _service.AddHutToTrailAsync(trailHutDto);
            }
            catch (AggregateException aex)
            {
                return NotFound(String.Join("\n", aex.InnerExceptions.Select(iex => iex.Message)));
            }
            catch (InvalidOperationException iex)
            {
                return BadRequest(iex.Message);
            }

            return Ok(trailHutDto);
        }

        // DELETE: /api/trailhut/{body}
        [HttpDelete]
        public async Task<ActionResult> DeleteHutToTrail(TrailHutDto trailHutDto)
        {
            try
            {
                await _service.RemoveHutFromTrailAsync(trailHutDto);
            }
            catch (AggregateException aex)
            {
                return NotFound(String.Join("\n", aex.InnerExceptions.Select(iex => iex.Message)));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Hut is successfully removed from the Trail!");
        }
    }
}
