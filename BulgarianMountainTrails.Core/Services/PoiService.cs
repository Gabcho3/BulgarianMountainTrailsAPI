using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Helpers;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using BulgarianMountainTrails.Data.Enums;

namespace BulgarianMountainTrails.Core.Services
{
    public class PoiService : IPoiService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<PointOfInterest> _validator;

        public PoiService(ApplicationDbContext context, IMapper mapper, IValidator<PointOfInterest> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
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

                pois = pois.Where(p => p.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();

                if (pois.Count == 0)
                    throw new KeyNotFoundException($"No POIs of type '{type}' found!");
            }

            return pois;
        }

        public async Task<PointOfInterest> GetPOIByIdAsync(Guid id)
        {
            var poi = await _context.PointsOfInterest
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (poi == null)
                throw new KeyNotFoundException("POI not found!");

            return poi!;
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

        public async Task CreatePOIAsync(PoiDto dto)
        {
          if (dto == null)
                throw new ArgumentNullException(nameof(dto), "POI data is required!");

            PointOfInterest poi;
            try
            {
                poi = dto.Type.ToLower() switch
                {
                    "river" => _mapper.Map<River>(dto),
                    "lake" => _mapper.Map<Lake>(dto),
                    "waterfall" => _mapper.Map<Waterfall>(dto),
                    "peak" => _mapper.Map<Peak>(dto),
                    "monastery" => _mapper.Map<Monastery>(dto),
                    "cave" => _mapper.Map<Cave>(dto),
                    _ => throw new ArgumentException($"Invalid POI type '{dto.Type}'. Valid types are: {string.Join(", ", Enum.GetNames(typeof(PoiType)))}")
                };
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Error parsing POI data. Please ensure the data is in the correct format.", ex);
            }

            var validationResult = await _validator.ValidateAsync(poi);
            if (!validationResult.IsValid)
            {
                var errors = new List<ApiError>();

                foreach (var error in validationResult.Errors)
                {
                    errors.Add(new ApiError
                    {
                        Field = error.PropertyName,
                        Message = error.ErrorMessage
                    });
                }

                throw new ApiException(errors);
            }

            await _context.PointsOfInterest.AddAsync(poi);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task DeletePOIAsync(Guid id)
        {
            var poi = await _context.PointsOfInterest.FindAsync(id);

            if (poi == null)
                throw new KeyNotFoundException("POI not found!");

            _context.PointsOfInterest.Remove(poi);
            await _context.SaveChangesAsync();

            return;
        }
    }
}
