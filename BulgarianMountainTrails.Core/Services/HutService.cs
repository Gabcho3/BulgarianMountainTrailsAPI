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

        public async Task<IEnumerable<HutDto>> GetAllAsync(int? minAltitude, int? maxAltitude, int? minCapacity, int? maxCapacity, string? mountain)
        {
            var query = FilterHuts(minAltitude, maxAltitude, mountain, minCapacity, maxCapacity);

            return await query
                .AsNoTracking()
                .Include(h => h.TrailHuts)
                .ThenInclude(th => th.Trail)
                .Select(h => _mapper.Map<HutDto>(h))
                .ToListAsync();
        }

        public async Task<HutDto?> GetByIdAsync(Guid id)
        {
            var hut = await _context.Huts
                .AsNoTracking()
                .Include(t => t.TrailHuts)
                .ThenInclude(th => th.Trail)
                .FirstOrDefaultAsync(t => t.Id == id);

            return _mapper.Map<HutDto>(hut);
        }

        public Task<HutDto> CreateAsync(HutDto hutDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            var hut = await _context.Huts.FindAsync(id);

            if (hut == null)
            {
                throw new ArgumentException("There is not a Hut with this Id!");
            }

            _context.Remove(hut);
            await _context.SaveChangesAsync();

            return;
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
