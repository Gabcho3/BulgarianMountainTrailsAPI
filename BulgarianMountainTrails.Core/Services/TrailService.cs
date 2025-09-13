using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using BulgarianMountainTrails.Data.Enums;

using BulgarianMountainTrails.Core.Helpers;

namespace BulgarianMountainTrails.Core.Services
{
    public class TrailService : ITrailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<TrailDto> _validator;   

        public TrailService(ApplicationDbContext context, IMapper mapper, IValidator<TrailDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<TrailDto>> GetAllAsync(double? minHours, double? maxHours, double? minKm, double? maxKm, string? difficulty, string? mountain)
        {
            var query = FilterTrails(minHours, maxHours, minKm, maxKm, mountain, difficulty);

            var trails = await query
                .AsNoTracking()
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .Select(t => _mapper.Map<TrailDto>(t))
                .ToListAsync();

            if (trails.Count == 0)
                throw new KeyNotFoundException("No trails found matching the criteria!");

            return trails;
        }

        public async Task<TrailDto?> GetByIdAsync(Guid id)
        {
            var trail = await _context.Trails
                .AsNoTracking()
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trail == null)
                throw new KeyNotFoundException("Trail not found!");

            return _mapper.Map<TrailDto>(trail);
        }

        public async Task CreateAsync(TrailDto trailDto)
        {
            var validationResult = await _validator.ValidateAsync(trailDto);

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

            var trail = _mapper.Map<Trail>(trailDto);

            await _context.Trails.AddAsync(trail);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task DeleteAsync(Guid id)
        {
            var trail = await _context.Trails.FindAsync(id);

            if (trail == null)
            {
                throw new KeyNotFoundException("Trail not found!");
            }

            _context.Remove(trail);
            await _context.SaveChangesAsync();

            return;
        }

        private IQueryable<Trail> FilterTrails(double? minHours, double? maxHours, double? minKm, double? maxKm, string? mountain, string? difficulty)
        {
            var errors = new List<ApiError>();

            if (minHours > maxHours)
                errors.Add(new() { Field = "Hours", Message = "MinHours cannot be greater than MaxHours!" });

            if (minKm > maxKm)
                errors.Add(new() { Field = "Km", Message = "MinKm cannot be greater than MaxKm!}" });

            var query = _context.Trails.AsNoTracking().AsQueryable();

            if (difficulty != null)
            {
                bool isValidDifficulty = Enum.TryParse<DifficultyEnum>(difficulty, out var difficultyEnum);

                if (!isValidDifficulty || !Enum.IsDefined(typeof(DifficultyEnum), difficultyEnum))
                    errors.Add(new() { Field = "Difficulty Level", Message = "Invalid Difficulty Level! The valid values are: Unknown, Easy, Medium, Hard" });

                if(errors.Count > 0)
                    throw new ApiException(errors);

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
