using AutoMapper;
using FluentValidation;
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

            return await query
                .AsNoTracking()
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .Select(t => _mapper.Map<TrailDto>(t))
                .ToListAsync();
        }

        public async Task<TrailDto?> GetByIdAsync(Guid id)
        {
            var trail = await _context.Trails
                .AsNoTracking()
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Hut)
                .FirstOrDefaultAsync(t => t.Id == id);

            return _mapper.Map<TrailDto>(trail);
        }

        public async Task CreateAsync(TrailDto trailDto)
        {
            var validationResult = await _validator.ValidateAsync(trailDto);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Trail validation failed:\n{errors}");
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
