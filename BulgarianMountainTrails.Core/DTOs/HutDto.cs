namespace BulgarianMountainTrails.Core.DTOs
{
    public class HutDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Mountain { get; set; }
        public int Altitude { get; set; }
        public int Capacity { get; set; }
        public string? PhoneNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<SimpleTrailDto> Trails { get; set; } = new();
    }

    public class SimpleTrailDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Mountain { get; set; }
        public string Difficulty { get; set; } = null!;
        public double LengthKm { get; set; }
        public double DurationHours { get; set; }
        public string StartPoint { get; set; } = null!;
        public string EndPoint { get; set; } = null!;
        public string? Description { get; set; }
    }
}
