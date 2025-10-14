using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/trailHut")]
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
            var huts = await _service.GetHutsForTrailAsync(trailId);
            return Ok(huts);
        }

        // GET: /api/trailhut/hut/{id}
        [HttpGet("hut/{hutId}")]
        public async Task<ActionResult> GetTrailsForHut(Guid hutId)
        {
            var trails = await _service.GetTrailsForHutAsync(hutId);
            return Ok(trails);
        }

        // POST: /api/trailhut/{body}
        [HttpPost]
        public async Task<ActionResult> AddHutToTrail(TrailHutDto trailHutDto)
        {
            await _service.AddHutToTrailAsync(trailHutDto);
            return Ok(trailHutDto);
        }

        // DELETE: /api/trailhut?trailId={trailId}&hutId={hutId}
        [HttpDelete]
        public async Task<ActionResult> DeleteHutToTrail(Guid trailId, Guid hutId)
        {
            await _service.RemoveHutFromTrailAsync(trailId, hutId);
            return Ok("Hut is successfully removed from the Trail!");
        }
    }
}
