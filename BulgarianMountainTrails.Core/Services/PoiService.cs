using AutoMapper;
using BulgarianMountainTrails.Core.Interfaces;
using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using BulgarianMountainTrails.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        public async Task<IEnumerable<PointOfInterest>> GetPoisForTrailByTypeAsync(Guid trailId, string type)
        {
            if (!Enum.TryParse<PoiType>(type, true, out var poiType))
            {
                throw new ArgumentException($"Invalid POI type '{type}'. Valid types are: {string.Join(", ", Enum.GetNames(typeof(PoiType)))}");
            }

            var trail = await _context.Trails
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == trailId);

            if (trail == null)
                throw new KeyNotFoundException("Trail not found!");

            var pois = trail.TrailPOIs
                .Select(tp => tp.PointOfInterest)
                .Where(p => p.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (!pois.Any())
                throw new KeyNotFoundException($"No POIs of type '{type}' found for the specified trail!");

            return pois;
        }
    }
}
