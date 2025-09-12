using AutoMapper;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
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
                throw new ArgumentException("Trail not found");
            }

            return await _context.TrailHuts
                .Where(th => th.TrailId == trailId)
                .Select(th => _mapper.Map<SimpleHutDto>(th.Hut))
                .ToListAsync();
        }

        public Task<IEnumerable<SimpleTrailDto>> GetTrailsForHutAsync(Guid hutId)
        {
            throw new NotImplementedException();
        }

        public Task AddHutToTrailAsync(TrailHutDto trailHutDto)
        {
            throw new NotImplementedException();
        }

        public Task RemoveHutFromTrailAsync(TrailHutDto trailHutDto)
        {
            throw new NotImplementedException();
        }
    }
}
