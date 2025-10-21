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

        public async Task<IEnumerable<PointOfInterest>> GetAllPOIsAsync()
        {
            var pois = await _context.PointsOfInterest
                .AsNoTracking()
                .ToListAsync();

            if (pois.Count == 0)
                throw new KeyNotFoundException("No POIs found! The database might be empty!");

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

        public async Task<IEnumerable<River>> GetRiversAsync(double? minLength, double? maxLength)
        {
            var rivers = await _context.PointsOfInterest
                .AsNoTracking()
                .OfType<River>()
                .ToListAsync();

            CheckValues(new Dictionary<string, double?[]>
            {
                { "Length", new double?[] { minLength, maxLength } }
            });

            rivers = rivers.Where(r => (!minLength.HasValue || r.LengthKm >= minLength) && (!maxLength.HasValue || r.LengthKm <= maxLength)).ToList();

            IsEmpty(rivers.Count, "rivers");

            return rivers;
        }

        public async Task<IEnumerable<Lake>> GetLakesAsync(double? minArea, double? maxArea, double? minDepth, double? maxDepth)
        {
            var lakes = await _context.PointsOfInterest
                .AsNoTracking()
                .OfType<Lake>()
                .ToListAsync();

            var keyValuePairs = new Dictionary<string, double?[]>
            {
                { "Area", new double?[] { minArea , maxArea } },
                { "Depth", new double?[] { minDepth, maxDepth } }
            };
            CheckValues(keyValuePairs);

            lakes = lakes.Where(l => (!minArea.HasValue || l.AreaKm2 >= minArea) && (!maxArea.HasValue || l.AreaKm2 <= maxArea)).ToList();
            lakes = lakes.Where(l => (!minDepth.HasValue || l.DepthM >= minDepth) && (!maxDepth.HasValue || l.DepthM <= maxDepth)).ToList();

            IsEmpty(lakes.Count, "lakes");

            return lakes;
        }

        public async Task<IEnumerable<Waterfall>> GetWaterfallsAsync(double? minHeight, double? maxHeight)
        {
            var waterfalls = await _context.PointsOfInterest
                .AsNoTracking()
                .OfType<Waterfall>()
                .ToListAsync();

            CheckValues(new Dictionary<string, double?[]>
            {
                { "Height", new double?[] { minHeight , maxHeight} }
            });

            waterfalls = waterfalls.Where(l => (!minHeight.HasValue || l.HeightM >= minHeight) && (!maxHeight.HasValue || l.HeightM <= maxHeight)).ToList();

            IsEmpty(waterfalls.Count, "waterfalls");

            return waterfalls;
        }

        public async Task<IEnumerable<Peak>> GetPeaksAsync(double? minElevation, double? maxElevation)
        {
            var peaks = await _context.PointsOfInterest
                .AsNoTracking()
                .OfType<Peak>()
                .ToListAsync();

            CheckValues(new Dictionary<string, double?[]>
            {
                { "Elevation", new double?[] { minElevation , maxElevation} }
            });

            peaks = peaks.Where(p => (!minElevation.HasValue || p.ElevationM >= minElevation) && (!maxElevation.HasValue || p.ElevationM <= maxElevation)).ToList();

            IsEmpty(peaks.Count, "peaks");

            return peaks;
        }

        public async Task<IEnumerable<Monastery>> GetMonasteriesAsync(int? foundedAfterYear, int? foundedBeforeYear)
        {
            var monasteries = await _context.PointsOfInterest
                .AsNoTracking()
                .OfType<Monastery>()
                .ToListAsync();

            CheckValues(new Dictionary<string, double?[]>
            {
                { "Founded year", new double?[] { foundedAfterYear , foundedBeforeYear} }
            });

            monasteries = monasteries.Where(m => (!foundedAfterYear.HasValue || m.FoundedYear >= foundedAfterYear) && (!foundedBeforeYear.HasValue || m.FoundedYear <= foundedBeforeYear)).ToList();

            IsEmpty(monasteries.Count, "monasteries");

            return monasteries;
        }

        public async Task<IEnumerable<Cave>> GetCavesAsync(double? minLength, double? maxLength, bool? isAccessible)
        {
            var caves = await _context.PointsOfInterest
                .AsNoTracking()
                .OfType<Cave>()
                .ToListAsync();

            CheckValues(new Dictionary<string, double?[]>
            {
                { "Length", new double?[] { minLength, maxLength } }
            });

            caves = caves.Where(c => (!minLength.HasValue || c.LengthM >= minLength) 
                && (!maxLength.HasValue || c.LengthM <= maxLength)
                && (!isAccessible.HasValue || c.IsTouristAccessible == isAccessible))
                .ToList();

            IsEmpty(caves.Count, "caves");

            return caves;
        }

        private void CheckValues(Dictionary<string, double?[]> keyValuePairs)
        {
            foreach (var pair in keyValuePairs)
            {
                if (pair.Value[0].HasValue && pair.Value[1].HasValue)
                {
                    if (pair.Value[0] > pair.Value[1])
                        throw new ArgumentException($"Minimum {pair.Key} cannot be greater than maximum {pair.Key}.");
                }
            }
        }

        private bool IsEmpty(int count, string itemName)
            => count == 0 ? throw new KeyNotFoundException($"No {itemName} found the matching the specofied criteria! Database might be empty!") : false;
    }
}
