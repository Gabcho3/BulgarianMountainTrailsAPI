using System.ComponentModel.DataAnnotations;

namespace BulgarianMountainTrailsAPI.Data.Models
{
    public class TrailHut
    {
        [Required]
        public Guid TrailId { get; set; }

        public required Trail Trail { get; set; }

        [Required]
        public Guid HutId { get; set; }

        public required MountainHut Hut { get; set; }
    }
}
