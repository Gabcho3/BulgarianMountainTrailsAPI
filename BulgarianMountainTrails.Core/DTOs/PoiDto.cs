namespace BulgarianMountainTrails.Core.DTOs
{
    public class PoiDto
    {
        public string Type { get; set; } = null!;
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Mountain { get; set; }

        //Extra fields for specific POI types
        public double? LengthKm { get; set; } //For rivers

        public double? AreaSqKm { get; set; } //For lakes
        public double? DepthM { get; set; } //For lakes

        public double? HeightM { get; set; } //For waterfalls

        public int? ElevationM { get; set; } //For peaks

        public int? FoundationYear { get; set; } //For monasteries

        public double? LengthM { get; set; } //For caves
        public bool? IsTouristAccessible { get; set; } //For caves
    }
}
