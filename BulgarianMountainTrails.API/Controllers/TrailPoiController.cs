using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/trailPoi")]
    [ApiController]
    public class TrailPoiController : ControllerBase
    {
        private readonly ITrailPoiService _service;

        public TrailPoiController(ITrailPoiService service)
        {
            _service = service;
        }

        // GET: /api/trailPoi/trail/{id}
        [HttpGet("trail/{trailId}")]
        public async Task<ActionResult> GetPoisForTrail(Guid trailId)
        {
            var pois = await _service.GetPoisForTrailAsync(trailId);
            return Ok(pois);
        }

        // GET: /api/trailPoi/poi/{id}
        [HttpGet("poi/{poiId}")]
        public async Task<ActionResult> GetTrailsForPoi(Guid poiId)
        {
            var trails = await _service.GetTrailsForPoiAsync(poiId);
            return Ok(trails);
        }

        // POST: /api/trailPoi/{body}
        [HttpPost]
        public async Task<ActionResult> AddPoiToTrail(TrailPoiDto trailPoiDto)
        {
            await _service.AddPoiToTrailAsync(trailPoiDto);
            return Ok(trailPoiDto);
        }

        // DELETE: /api/trailPoi?trailId={trailId}&poiId={poiId}
        [HttpDelete]
        public async Task<ActionResult> DeletePoiToTrail(Guid trailId, Guid poiId)
        {
            await _service.RemovePoiFromTrailAsync(trailId, poiId);
            return Ok("POI is successfully removed from the Trail!");
        }
    }
}
