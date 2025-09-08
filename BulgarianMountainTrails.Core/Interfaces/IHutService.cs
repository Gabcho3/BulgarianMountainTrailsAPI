using BulgarianMountainTrails.Core.DTOs;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface IHutService
    {
        Task<IEnumerable<HutDto>> GetAllAsync(int? minAltitude, int? maxAltitude, int? minCapacity, int? maxCapacity, string? mountain);

        Task<HutDto?> GetByIdAsync(Guid id);

        Task<HutDto> CreateAsync(HutDto hutDto);

        Task DeleteAsync(Guid id);
    }
}
