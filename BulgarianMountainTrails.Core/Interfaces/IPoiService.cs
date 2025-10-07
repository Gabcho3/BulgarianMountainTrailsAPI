using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface IPoiService
    {
        Task<IEnumerable<PointOfInterest>> GetAllPOIsAsync(string? type);

        Task<IEnumerable<PointOfInterest>> GetPOIsForTrailAsync(Guid trailId, string? type);
    }
}
