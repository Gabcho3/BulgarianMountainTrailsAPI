using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface IPoiService
    {
        Task<IEnumerable<PointOfInterest>> GetPOIsForTrailAsync(Guid trailId);

        Task<IEnumerable<PointOfInterest>> GetPoisForTrailByTypeAsync(Guid trailId, string type);
    }
}
