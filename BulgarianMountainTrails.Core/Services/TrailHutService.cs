using AutoMapper;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrails.Core.Services
{
    public class TrailHutService : ITrailHutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrailHutService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SimpleHutDto>> GetHutsForTrailAsync(Guid trailId)
        {
            var trail = await _context.Trails.FindAsync(trailId);

            if (trail == null)
            {
                throw new ArgumentException("Trail not found!");
            }

            return await _context.TrailHuts
                .Where(th => th.TrailId == trailId)
                .Select(th => _mapper.Map<SimpleHutDto>(th.Hut))
                .ToListAsync();
        }

        public async Task<IEnumerable<SimpleTrailDto>> GetTrailsForHutAsync(Guid hutId)
        {
            var hut = await _context.Huts.FindAsync(hutId);

            if (hut == null)
            {
                throw new ArgumentException("Hut not found!");
            }

            return await _context.TrailHuts
                .Where(th => th.HutId == hutId)
                .Select(th => _mapper.Map<SimpleTrailDto>(th.Trail))
                .ToListAsync();
        }

        public async Task AddHutToTrailAsync(TrailHutDto trailHutDto)
        {
            var trail = await _context.Trails.FindAsync(trailHutDto.TrailId);
            var hut = await _context.Huts.FindAsync(trailHutDto.HutId);

            if (trail == null )
            {
                throw new ArgumentException("Trail not found!");
            }

            if (hut == null)
            {
                throw new ArgumentException("Hut not found!");
            }

            var existingAssociation = await _context.TrailHuts
                .AnyAsync(th => th.TrailId == trailHutDto.TrailId && th.HutId == trailHutDto.HutId);

            if (existingAssociation)
            {
                throw new ArgumentException("This Hut is already associated with the Trail!");
            }

            var trailHut = _mapper.Map<TrailHut>(trailHutDto);

            await _context.TrailHuts.AddAsync(trailHut);
            await _context.SaveChangesAsync();

            return;
        }

        public Task RemoveHutFromTrailAsync(TrailHutDto trailHutDto)
        {
            throw new NotImplementedException();
        }
    }
}
