using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface ITrailService
    {
        Task<IEnumerable<TrailDto>> GetAllAsync(double? minHours, double? maxHours, double? minKm, double? maxKm, string? difficulty, string? mountain);

        Task<TrailDto?> GetByIdAsync(Guid id);

        Task<TrailDto> CreateAsync(Trail trail);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<TrailDto>> SearchAsync(int? maxHours, string? difficulty, string? mountain);
    }
}
