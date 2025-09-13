using AutoMapper;
using Microsoft.EntityFrameworkCore;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;

using BulgarianMountainTrails.Core.Helpers;

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
                throw new KeyNotFoundException("Trail not found!");

            var huts = await _context.TrailHuts
                .Where(th => th.TrailId == trailId)
                .Select(th => _mapper.Map<SimpleHutDto>(th.Hut))
                .ToListAsync();

            if (huts.Count == 0)
                throw new KeyNotFoundException("No Huts found for this Trail!");

            return huts;
        }

        public async Task<IEnumerable<SimpleTrailDto>> GetTrailsForHutAsync(Guid hutId)
        {
            var hut = await _context.Huts.FindAsync(hutId);

            if (hut == null)
                throw new KeyNotFoundException("Hut not found!");

            var trails = await _context.TrailHuts
                .Where(th => th.HutId == hutId)
                .Select(th => _mapper.Map<SimpleTrailDto>(th.Trail))
                .ToListAsync();

            if (trails.Count == 0)
                throw new KeyNotFoundException("No Trails found for this Hut!");

            return trails;
        }

        public async Task AddHutToTrailAsync(TrailHutDto trailHutDto)
        {
            await TrailHutExistsAsync(trailHutDto.TrailId, trailHutDto.HutId);

            var existingAssociation = await _context.TrailHuts
                .AnyAsync(th => th.TrailId == trailHutDto.TrailId && th.HutId == trailHutDto.HutId);

            if (existingAssociation)
                throw new InvalidOperationException("This Hut is already associated with the Trail!");

            var trailHut = _mapper.Map<TrailHut>(trailHutDto);

            await _context.TrailHuts.AddAsync(trailHut);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task RemoveHutFromTrailAsync(TrailHutDto trailHutDto)
        {
            await TrailHutExistsAsync(trailHutDto.TrailId, trailHutDto.HutId);

            var trailHut = _context.TrailHuts
                .FirstOrDefault(th => th.TrailId == trailHutDto.TrailId && th.HutId == trailHutDto.HutId);

            if (trailHut == null)
                throw new ArgumentException("This Hut is not associated with the Trail!");

            _context.TrailHuts.Remove(trailHut);
            await _context.SaveChangesAsync();

            return;
        }

        private async Task TrailHutExistsAsync(Guid trailId, Guid hutId)
        {
            var errors = new List<ApiError>();

            var trail = await _context.Trails.FindAsync(trailId);
            if (trail == null)
               errors.Add(new() { Field = "Trail", Message = "Trail not found!" });

            var hut = await _context.Huts.FindAsync(hutId);
            if (hut == null)
                errors.Add(new() { Field = "Hut", Message = "Hut not found!" });

            if (errors.Count > 0)
                throw new ApiException(errors);

            return;
        }
    }
}
