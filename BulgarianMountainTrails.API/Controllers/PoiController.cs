using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.Interfaces;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/poi")]
    [ApiController]
    public class PoiController : Controller
    {
        private readonly IPoiService _poiService;

        public PoiController(IPoiService poiService)
        {
            _poiService = poiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPOIs([FromQuery] string? type)
        {
            var pois = await _poiService.GetAllPOIsAsync(type);
            return Ok(pois);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPOIById(Guid id)
        {
            var poi = await _poiService.GetPOIByIdAsync(id);

            return Ok(poi);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreatePOI(PointOfInterest poi)
        //{
        //    var createdPoi = await _poiService.CreatePOIAsync(poi);

        //    return CreatedAtAction(nameof(GetPOIById), new { id = createdPoi.Id }, createdPoi);
        //}

        [HttpGet("trail/{trailId}")]
        public async Task<IActionResult> GetPOIsForTrail(Guid trailId, [FromQuery] string? type)
        {
            var pois = await _poiService.GetPOIsForTrailAsync(trailId, type);
            return Ok(pois);
        }
    }
}
