using System.ComponentModel.DataAnnotations;

namespace BulgarianMountainTrails.Data.Entities
{
    public class Hut
    {
        public Hut()
        {
            Id = Guid.NewGuid();
            TrailHuts = new List<TrailHut>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Mountain { get; set; }

        [Required]
        public int Altitude { get; set; }

        public int Capacity { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public ICollection<TrailHut> TrailHuts { get; set; }
    }
}
