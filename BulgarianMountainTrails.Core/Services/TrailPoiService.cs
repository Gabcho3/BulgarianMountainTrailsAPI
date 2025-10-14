using AutoMapper;
using Microsoft.EntityFrameworkCore;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Helpers;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Services
{
    public class TrailPoiService : ITrailPoiService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrailPoiService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PoiDto>> GetPoisForTrailAsync(Guid trailId)
        {
            var trail = await _context.Trails.FindAsync(trailId);

            if (trail == null)
                throw new KeyNotFoundException("Trail not found!");

            var pois = await _context.TrailPOIs
                .Where(th => th.TrailId == trailId)
                .Select(th => _mapper.Map<PoiDto>(th.PointOfInterest))
                .ToListAsync();

            if (pois.Count == 0)
                throw new KeyNotFoundException("No Pois found for this Trail!");

            return pois;
        }

        public async Task<IEnumerable<SimpleTrailDto>> GetTrailsForPoiAsync(Guid poiId)
        {
            var poi = await _context.PointsOfInterest.FindAsync(poiId);

            if (poi == null)
                throw new KeyNotFoundException("Poi not found!");

            var trails = await _context.TrailPOIs
                .Where(th => th.PointOfInterestId == poiId)
                .Select(th => _mapper.Map<SimpleTrailDto>(th.Trail))
                .ToListAsync();

            if (trails.Count == 0)
                throw new KeyNotFoundException("No Trails found for this POI!");

            return trails;
        }

        public async Task AddPoiToTrailAsync(TrailPoiDto trailPoiDto)
        {
            await TrailPoiExistsAsync(trailPoiDto.TrailId, trailPoiDto.PoiId);

            var existingAssociation = await _context.TrailPOIs
                .AnyAsync(th => th.TrailId == trailPoiDto.TrailId && th.PointOfInterestId == trailPoiDto.PoiId);

            if (existingAssociation)
                throw new InvalidOperationException("This POI is already associated with the Trail!");

            var trailPoi = _mapper.Map<TrailPOI>(trailPoiDto);

            await _context.TrailPOIs.AddAsync(trailPoi);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task RemovePoiFromTrailAsync(Guid trailId, Guid poiId)
        {
            await TrailPoiExistsAsync(trailId, poiId);

            var trailPoi = _context.TrailPOIs
                .FirstOrDefault(th => th.TrailId == trailId && th.PointOfInterestId == poiId);

            if (trailPoi == null)
                throw new ArgumentException("This POI is not associated with the Trail!");

            _context.TrailPOIs.Remove(trailPoi);
            await _context.SaveChangesAsync();

            return;
        }

        private async Task TrailPoiExistsAsync(Guid trailId, Guid poiId)
        {
            var errors = new List<ApiError>();

            var trail = await _context.Trails.FindAsync(trailId);
            if (trail == null)
                errors.Add(new() { Field = "Trail", Message = "Trail not found!" });

            var poi = await _context.PointsOfInterest.FindAsync(poiId);
            if (poi == null)
                errors.Add(new() { Field = "POI", Message = "POI not found!" });

            if (errors.Count > 0)
                throw new ApiException(errors);

            return;
        }
    }
}
