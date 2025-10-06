using AutoMapper;
using Microsoft.EntityFrameworkCore;

using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Services
{
    public class PoiService : IPoiService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PoiService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PointOfInterest>> GetPOIsForTrailAsync(Guid trailId)
        {
            var trailExists = await _context.Trails
                .AsNoTracking()
                .AnyAsync(t => t.Id == trailId);

            if (!trailExists)
                throw new KeyNotFoundException("Trail not found!");

            var pois = await _context.TrailPOIs
                .AsNoTracking()
                .Where(tp => tp.TrailId == trailId)
                .Select(tp => tp.PointOfInterest)
                .ToListAsync();

            if (pois.Count == 0)
                throw new KeyNotFoundException("No POIs found for the specified trail!");

            return pois;
        }
    }
}
