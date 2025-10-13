using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface IPoiService
    {
        Task<IEnumerable<PointOfInterest>> GetAllPOIsAsync(string? type);

        Task<PointOfInterest> GetPOIByIdAsync(Guid id);

        Task CreatePOIAsync(PoiDto dto);

        Task<IEnumerable<PointOfInterest>> GetPOIsForTrailAsync(Guid trailId, string? type);
    }
}
