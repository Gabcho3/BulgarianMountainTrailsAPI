using AutoMapper;
using FluentValidation;

using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Interfaces;

using BulgarianMountainTrails.Data;
using BulgarianMountainTrails.Data.Entities;
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

        public async Task<IEnumerable<HutDto>> GetAllHutsAsync(int? minAltitude, int? maxAltitude, int? minCapacity, int? maxCapacity, string? mountain)
        {
            var query = FilterHuts(minAltitude, maxAltitude, mountain, minCapacity, maxCapacity);

            return await query
                .AsNoTracking()
                .Include(h => h.TrailHuts)
                .ThenInclude(th => th.Trail)
                .Select(h => _mapper.Map<HutDto>(h))
                .ToListAsync();
        }

        public Task<HutDto?> GetHutByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<HutDto> CreateHutAsync(HutDto hutDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteHutAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Hut> FilterHuts(int? minAltitude, int? maxAltitude, string? mountain, int? minCapacity, int? maxCapacity)
        {
            if (minAltitude > maxAltitude)
                throw new ArgumentException("MinAltitude cannot be greater than MaxAltitude!");

            if (minCapacity > maxCapacity)
                throw new ArgumentException("MinCapacity cannot be greater than MaxCapacity!");

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
