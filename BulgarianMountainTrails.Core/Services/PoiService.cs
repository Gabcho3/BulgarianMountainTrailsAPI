using AutoMapper;
using BulgarianMountainTrails.Core.Interfaces;
using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using BulgarianMountainTrails.Data.Enums;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<PointOfInterest>> GetAllPOIsAsync(string? type)
        {
            var pois = await _context.PointsOfInterest
                .AsNoTracking()
                .ToListAsync();

            if (pois.Count == 0)
                throw new KeyNotFoundException("No POIs found! The database might be empty!");

            if (type != null)
            {
                if (!Enum.TryParse<PoiType>(type, true, out var poiType))
                {
                    throw new ArgumentException($"Invalid POI type '{type}'. Valid types are: {string.Join(", ", Enum.GetNames(typeof(PoiType)))}");
                }

                pois.Where(p => p.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase));

                if (pois.Count == 0)
                    throw new KeyNotFoundException($"No POIs of type '{type}' found!");
            }

            return pois;
        }

        public async Task<IEnumerable<PointOfInterest>> GetPOIsForTrailAsync(Guid trailId, string? type)
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

            if (type != null)
            {
                if (!Enum.TryParse<PoiType>(type, true, out var poiType))
                {
                    throw new ArgumentException($"Invalid POI type '{type}'. Valid types are: {string.Join(", ", Enum.GetNames(typeof(PoiType)))}");
                }

                pois.Where(p => p.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase));

                if (pois.Count == 0)
                    throw new KeyNotFoundException($"No POIs of type '{type}' found!");
            }

            if (pois.Count == 0)
                throw new KeyNotFoundException("No POIs found for the specified trail!");

            return pois;
        }
    }
}
