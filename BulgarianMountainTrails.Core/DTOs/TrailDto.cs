namespace BulgarianMountainTrails.Core.DTOs
{
    public class TrailDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }

        public required string Mountain { get; set; }

        public string Difficulty { get; set; } = null!;

        public double LengthKm { get; set; }

        public double DurationHours { get; set; }

        public string StartPoint { get; set; } = null!;

        public string EndPoint { get; set; } = null!;

        public string? Description { get; set; }

        public List<SimpleHutDto> Huts { get; set; } = new();
    }

    public class SimpleHutDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Mountain { get; set; }
        public int Altitude { get; set; }
        public int Capacity { get; set; }
        public string? PhoneNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
