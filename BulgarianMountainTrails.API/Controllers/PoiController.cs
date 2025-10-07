using Microsoft.AspNetCore.Mvc;

using BulgarianMountainTrails.Core.Interfaces;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("trail/{trailId}")]
        public async Task<IActionResult> GetPOIsForTrail(Guid trailId, [FromQuery] string? type)
        {
            var pois = await _poiService.GetPOIsForTrailAsync(trailId, type);
            return Ok(pois);
        }
    }
}
