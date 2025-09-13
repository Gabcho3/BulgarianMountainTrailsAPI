using AutoMapper;
using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Helpers;
using BulgarianMountainTrails.Core.Interfaces;
using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrails.Core.Services
{
    public class HutService : IHutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<HutDto> _validator;

        public HutService(ApplicationDbContext context, IMapper mapper, IValidator<HutDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<HutDto>> GetAllAsync(int? minAltitude, int? maxAltitude, int? minCapacity, int? maxCapacity, string? mountain)
        {
            var query = FilterHuts(minAltitude, maxAltitude, mountain, minCapacity, maxCapacity);

            var huts = await query
                .AsNoTracking()
                .Include(h => h.TrailHuts)
                .ThenInclude(th => th.Trail)
                .Select(h => _mapper.Map<HutDto>(h))
                .ToListAsync();

            if (huts.Count == 0)
                throw new KeyNotFoundException("No huts found matching the criteria!");

            return huts;
        }

        public async Task<HutDto?> GetByIdAsync(Guid id)
        {
            var hut = await _context.Huts
                .AsNoTracking()
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Trail)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (hut == null)
                throw new KeyNotFoundException("Hut not found!");

            return _mapper.Map<HutDto>(hut);
        }

        public async Task CreateAsync(HutDto hutDto)
        {
            var validationResult = await _validator.ValidateAsync(hutDto);

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

            var hut = _mapper.Map<Hut>(hutDto);

            await _context.Huts.AddAsync(hut);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task DeleteAsync(Guid id)
        {
            var hut = await _context.Huts.FindAsync(id);

            if (hut == null)
                throw new KeyNotFoundException("Hut not found!");

            _context.Remove(hut);
            await _context.SaveChangesAsync();

            return;
        }

        private IQueryable<Hut> FilterHuts(int? minAltitude, int? maxAltitude, string? mountain, int? minCapacity, int? maxCapacity)
        {
            var errors = new List<ApiError>();

            if (minAltitude > maxAltitude)
                errors.Add(new ApiError { Field = "Altitude", Message = "MinAltitude cannot be greater than MaxAltitude!" });

            if (minCapacity > maxCapacity)
                errors.Add(new ApiError { Field = "Capacity", Message = "MinCapacity cannot be greater than Capacity!" });

            if (errors.Count > 0)
                throw new ApiException(errors);

            var query = _context.Huts.AsNoTracking().AsQueryable();

            if (minAltitude.HasValue)
                query = query.Where(h => h.Altitude >= minAltitude.Value);

            if (maxAltitude.HasValue)
                query = query.Where(h => h.Altitude <= maxAltitude.Value);

            if (!string.IsNullOrEmpty(mountain))
                query = query.Where(h => h.Mountain.ToLower() == mountain.ToLower());

            if (minCapacity.HasValue)
                query = query.Where(h => h.Capacity >= minCapacity.Value);

            if (maxCapacity.HasValue)
                query = query.Where(h => h.Capacity <= maxCapacity.Value);

            return query;
        }
    }
}
