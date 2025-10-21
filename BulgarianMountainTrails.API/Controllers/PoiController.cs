using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        //Routing for all POIs

        //GET: /api/poi?type=
        [HttpGet]
        public async Task<IActionResult> GetAllPOIs()
        {
            var pois = await _poiService.GetAllPOIsAsync();
            return Ok(pois);
        }

        //GET: /api/poi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPOIById(Guid id)
        {
            var poi = await _poiService.GetPOIByIdAsync(id);
            return Ok(poi);
        }

        //GET: /api/poi/trail/{trailId}?type=
        [HttpGet("trail/{trailId}")]
        public async Task<IActionResult> GetPOIsForTrail(Guid trailId, [FromQuery] string? type)
        {
            var pois = await _poiService.GetPOIsForTrailAsync(trailId, type);
            return Ok(pois);
        }

        // POST: /api/poi
        [HttpPost]
        public async Task<IActionResult> CreatePOI(PoiDto dto)
        {
            await _poiService.CreatePOIAsync(dto);
            return CreatedAtAction(nameof(GetPOIById), new { id = dto.Id }, dto);
        }

        // DELETE: /api/poi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePOIAsync(Guid id)
        {
            await _poiService.DeletePOIAsync(id);
            return Ok("Successfully deleted!");
        }

        //Routing for specific POI types 
        [HttpGet("rivers")]
        public async Task<IActionResult> GetAllRivers([FromQuery] double? minLength, double? maxLength)
        {
            var rivers = await _poiService.GetRiversAsync(minLength, maxLength);
            return Ok(rivers);
        }

        [HttpGet("lakes")]
        public async Task<IActionResult> GetAllLakes([FromQuery] double? minArea, double? maxArea, double? minDepth, double? maxDepth)
        {
            var lakes = await _poiService.GetLakesAsync(minArea, maxArea, minDepth, maxDepth);
            return Ok(lakes);
        }

        [HttpGet("waterfalls")]
        public async Task<IActionResult> GetAllWaterfalls([FromQuery] double? minHeight, double? maxHeight)
        {
            var waterfalls = await _poiService.GetWaterfallsAsync(minHeight, maxHeight);
            return Ok(waterfalls);
        }

        [HttpGet("peaks")]
        public async Task<IActionResult> GetAllPeaks([FromQuery] int? minElevation, int? maxElevation)
        {
            var peaks = await _poiService.GetPeaksAsync(minElevation, maxElevation);
            return Ok(peaks);
        }

        [HttpGet("monasteries")]
        public async Task<IActionResult> GetAllMonasteries([FromQuery] int? after, int? before)
        {
            var monastery = await _poiService.GetMonasteriesAsync(after, before);
            return Ok(monastery);
        }

        [HttpGet("caves")]
        public async Task<IActionResult> GetAllCaves([FromQuery] int? minLength, int? maxLength, bool? accessible)
        {
            var caves = await _poiService.GetCavesAsync(minLength, maxLength, accessible);
            return Ok(caves);
        }
    }
}
