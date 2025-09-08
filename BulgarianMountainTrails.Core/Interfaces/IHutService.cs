using BulgarianMountainTrails.Core.DTOs;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface IHutService
    {
        Task<IEnumerable<HutDto>> GetAllHutsAsync(int? minAltitude, int? maxAltitude, int? minCapacity, int? maxCapacity, string? mountain);

        Task<HutDto?> GetHutByIdAsync(Guid id);

        Task<HutDto> CreateHutAsync(HutDto hutDto);

        Task DeleteHutAsync(Guid id);
    }
}
