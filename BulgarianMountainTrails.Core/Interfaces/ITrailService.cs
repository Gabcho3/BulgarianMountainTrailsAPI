using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface ITrailService
    {
        Task<IEnumerable<TrailDto>> GetAllAsync(double? minHours, double? maxHours, double? minKm, double? maxKm, string? difficulty, string? mountain);

        Task<TrailDto?> GetByIdAsync(Guid id);

        Task CreateAsync(TrailDto trailDto);

        Task<bool> DeleteAsync(Guid id);
    }
}
