using System.ComponentModel.DataAnnotations;

namespace BulgarianMountainTrails.Data.Entities
{
    public abstract class PointOfInterest
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string? Mountain { get; set; }
    }

    public class River : PointOfInterest
    {
        [Required]
        public double LengthKm { get; set; }
    }

    public class Lake : PointOfInterest
    {
        [Required]
        public double AreaKm2 { get; set; }

        [Required]
        public double DepthM { get; set; }
    }

    public class Waterfall : PointOfInterest
    {
        [Required]
        public double HeightM { get; set; }
    }

    public class Peak : PointOfInterest
    {
        [Required]
        public int ElevationM { get; set; }
    }

    public class Monastery : PointOfInterest
    {
        [Required]
        public int FoundedYear { get; set; }
    }

    public class Cave : PointOfInterest
    {
        [Required]
        public double LengthM { get; set; }

        [Required]
        public bool IsTouristAccessible { get; set; }
    }

}
