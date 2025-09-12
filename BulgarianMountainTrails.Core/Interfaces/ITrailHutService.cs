using BulgarianMountainTrails.Core.DTOs;

namespace BulgarianMountainTrails.Core.Interfaces
{
    public interface ITrailHutService
    {
        Task<IEnumerable<SimpleHutDto>> GetHutsForTrailAsync(Guid trailId);

        Task<IEnumerable<SimpleTrailDto>> GetTrailsForHutAsync(Guid hutId);

        Task AddHutToTrailAsync(TrailHutDto trailHutDto);

        Task RemoveHutFromTrailAsync(TrailHutDto trailHutDto);
    }
}
