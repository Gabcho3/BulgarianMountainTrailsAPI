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

        [HttpGet("trail/{trailId}")]
        public async Task<IActionResult> GetPOIsForTrail(Guid trailId)
        {
            var pois = await _poiService.GetPOIsForTrailAsync(trailId);
            return Ok(pois);
        }
    }
}
