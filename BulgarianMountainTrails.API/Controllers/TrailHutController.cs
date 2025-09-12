using AutoMapper;
using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;
using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailHutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITrailHutService _service;

        public TrailHutController(ApplicationDbContext context, IMapper mapper, ITrailHutService service)
        {
            _context = context;
            _mapper = mapper;
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

                return Ok(trailHutDto);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "Trail not found!" || ex.Message == "Hut not found!")
                    return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        // DELETE: /api/trailhut/{body}
        [HttpDelete]
        public async Task<ActionResult> DeleteHutToTrail(TrailHutDto trailHutDto)
        {
            var trailHut = _mapper.Map<TrailHut>(trailHutDto);

            _context.TrailHuts.Remove(trailHut);
            await _context.SaveChangesAsync();

            return Ok(trailHut);
        }
    }
}
