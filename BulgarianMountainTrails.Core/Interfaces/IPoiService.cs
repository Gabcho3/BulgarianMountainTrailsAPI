using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface IPoiService
    {
        //Methods for all POI types
        Task<IEnumerable<PointOfInterest>> GetAllPOIsAsync(string? type);

        Task<PointOfInterest> GetPOIByIdAsync(Guid id);

        Task CreatePOIAsync(PoiDto dto);

        Task<IEnumerable<PointOfInterest>> GetPOIsForTrailAsync(Guid trailId, string? type);

        Task DeletePOIAsync(Guid id);

        //Methods for specific POI types
        Task<IEnumerable<PointOfInterest>> GetRiversAsync(double? minLength, double? maxLength);

        Task<IEnumerable<PointOfInterest>> GetLakesAsync(double? minArea, double? maxArea, double? minDepth, double? maxDepth);
    }
}
