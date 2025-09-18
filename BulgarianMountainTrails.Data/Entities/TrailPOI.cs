namespace BulgarianMountainTrails.Data.Entities
{
    public class TrailPOI
    {
        public Guid TrailId { get; set; }

        public Trail Trail { get; set; } = null!;

        public Guid PointOfInterestId { get; set; }

        public PointOfInterest PointOfInterest { get; set; } = null!;
    }
}
