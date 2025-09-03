using System.ComponentModel.DataAnnotations;

namespace BulgarianMountainTrails.Data.Entities
{
    public class TrailHut
    {
        public Guid TrailId { get; set; }

        public Trail? Trail { get; set; }

        public Guid HutId { get; set; }

        public Hut? Hut { get; set; }
    }
}
