using BulgarianMountainTrails.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BulgarianMountainTrails.Data.Entities
{
    public class Trail
    {
        public Trail()
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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DifficultyEnum Difficulty { get; set; }

        [Required]
        public double LengthKm { get; set; }

        [Required]
        public double DurationHours { get; set; }

        [Required]
        public string StartPoint { get; set; } = string.Empty;

        [Required]
        public string EndPoint { get; set; } = string.Empty;


        public string? Description { get; set; }

        public ICollection<TrailHut> TrailHuts { get; set; }
    }
}
