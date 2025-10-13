using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;
using BulgarianMountainTrails.Data.Entities;
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

        //GET: /api/poi?type=
        [HttpGet]
        public async Task<IActionResult> GetAllPOIs([FromQuery] string? type)
        {
            var pois = await _poiService.GetAllPOIsAsync(type);
            return Ok(pois);
        }

        //GET: /api/poi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPOIById(Guid id)
        {
            var poi = await _poiService.GetPOIByIdAsync(id);

            return Ok(poi);
        }

        // POST: /api/poi
        [HttpPost]
        public async Task<IActionResult> CreatePOI(PoiDto dto)
        {
            await _poiService.CreatePOIAsync(dto);

            return CreatedAtAction(nameof(GetPOIById), new { id = dto.Id }, dto);
        }

        // GET: /api/poi/trail/{trailId}?type=
        [HttpGet("trail/{trailId}")]
        public async Task<IActionResult> GetPOIsForTrail(Guid trailId, [FromQuery] string? type)
        {
            var pois = await _poiService.GetPOIsForTrailAsync(trailId, type);
            return Ok(pois);
        }
    }
}
