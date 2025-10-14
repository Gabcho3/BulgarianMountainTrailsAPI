using BulgarianMountainTrails.Core.DTOs;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface ITrailPoiService
    {
        Task<IEnumerable<PoiDto>> GetPoisForTrailAsync(Guid trailId);

        Task<IEnumerable<SimpleTrailDto>> GetTrailsForPoiAsync(Guid poiId);

        Task AddPoiToTrailAsync(TrailPoiDto trailPoiDto);

        Task RemovePoiFromTrailAsync(Guid trailId, Guid poiId);
    }
}
