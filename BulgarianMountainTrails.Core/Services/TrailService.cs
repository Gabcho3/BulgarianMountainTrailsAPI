using AutoMapper;
using Microsoft.EntityFrameworkCore;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using BulgarianMountainTrails.Data.Enums;

namespace BulgarianMountainTrails.Core.Services
{
    public class TrailService : ITrailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrailService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrailDto>> GetAllAsync(double? minHours, double? maxHours, double? minKm, double? maxKm, string? difficulty, string? mountain)
        {

            if (minHours.HasValue && maxHours.HasValue && minHours > maxHours)
                throw new ArgumentException("MinHours cannot be greater than MaxHours!");

            if (minKm.HasValue && maxKm.HasValue && minKm > maxKm)
                throw new ArgumentException("MinKm cannot be greater than MaxKm!");

            var query = FilterTrails(minHours, maxHours, minKm, maxKm, mountain, difficulty);

            return await query
                .AsNoTracking()
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .Select(t => _mapper.Map<TrailDto>(t))
                .ToListAsync();
        }

        public Task<TrailDto> CreateAsync(Trail trail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TrailDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrailDto>> SearchAsync(int? maxHours, string? difficulty, string? mountain)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Trail> FilterTrails(double? minHours, double? maxHours, double? minKm, double? maxKm, string? mountain, string? difficulty)
        {
            if (minHours > maxHours)
                throw new ArgumentException("MinHours cannot be greater than MaxHours!");

            if (minKm > maxKm)
                throw new ArgumentException("MinKm cannot be greater than MaxKm!");

            var query = _context.Trails.AsNoTracking().AsQueryable();

            if (difficulty != null)
            {
                bool isValidDifficulty = Enum.TryParse<DifficultyEnum>(difficulty, out var difficultyEnum);

                if (!isValidDifficulty || !Enum.IsDefined(typeof(DifficultyEnum), difficultyEnum))
                    throw new ArgumentException("Invalid Difficulty Level!\n" +
                        "The valid values are: Unknown, Easy, Medium, Hard");

                query = query.Where(t => t.Difficulty == difficultyEnum);
            }

            if (minHours.HasValue)
                query = query.Where(t => t.DurationHours >= minHours.Value);

            if (maxHours.HasValue)
                query = query.Where(t => t.DurationHours <= maxHours.Value);

            if (minKm.HasValue)
                query = query.Where(t => t.LengthKm >= minKm.Value);

            if (maxKm.HasValue)
                query = query.Where(t => t.LengthKm <= maxKm.Value);

            if (mountain != null)
                query = query.Where(t => EF.Functions.Like(t.Mountain, mountain));

            return query;
        }
    }
}
