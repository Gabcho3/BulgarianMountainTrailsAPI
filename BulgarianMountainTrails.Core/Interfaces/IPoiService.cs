using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface IPoiService
    {
        //Methods for all POI types
        Task<IEnumerable<PointOfInterest>> GetAllPOIsAsync();

        Task<PointOfInterest> GetPOIByIdAsync(Guid id);

        Task CreatePOIAsync(PoiDto dto);

        Task<IEnumerable<PointOfInterest>> GetPOIsForTrailAsync(Guid trailId, string? type);

        Task DeletePOIAsync(Guid id);

        //Methods for specific POI types
        Task<IEnumerable<River>> GetRiversAsync(double? minLength, double? maxLength);

        Task<IEnumerable<Lake>> GetLakesAsync(double? minArea, double? maxArea, double? minDepth, double? maxDepth);

        Task<IEnumerable<Waterfall>> GetWaterfallsAsync(double? minHeight, double? maxHeight);

        Task<IEnumerable<Peak>> GetPeaksAsync(double? minElevation, double? maxElevation);

        Task<IEnumerable<Monastery>> GetMonasteriesAsync(int? foundedAfterYear, int? foundedBeforeYear);

        Task<IEnumerable<Cave>> GetCavesAsync(double? minLength, double? maxLength, bool? isAccessible);
    }
}
