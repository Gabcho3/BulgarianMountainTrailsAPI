using AutoMapper;
using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailHutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrailHutController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /api/trailhut/trail/{id}
        [HttpGet("trail/{trailId}")]
        public async Task<ActionResult> GetHutsForTrail(Guid trailId)
        {
            var huts = await _context.TrailHuts
                .Where(th => th.TrailId == trailId)
                .Select(th => _mapper.Map<SimpleHutDto>(th.Hut))
                .ToListAsync();

            return Ok(huts);
        }

        // GET: /api/trailhut/hut/{id}
        [HttpGet("hut/{hutId}")]
        public async Task<ActionResult> GetTrailsForHut(Guid hutId)
        {
            var trails = await _context.TrailHuts
                .Where(th => th.HutId == hutId)
                .Select(th => _mapper.Map<SimpleTrailDto>(th.Trail))
                .ToListAsync();

            return Ok(trails);
        }

        // POST: /api/trailhut/{body}
        [HttpPost]
        public async Task<ActionResult> AddHutToTrail(TrailHut trailHut)
        {
            await _context.TrailHuts.AddAsync(trailHut);
            await _context.SaveChangesAsync();

            return Ok(trailHut);
        }

        // DELETE: /api/trailhut/{body}
        [HttpDelete]
        public async Task<ActionResult> DeleteHutToTrail(TrailHut trailHut)
        {
            _context.TrailHuts.Remove(trailHut);
            await _context.SaveChangesAsync();

            return Ok(trailHut);
        }
    }
}
