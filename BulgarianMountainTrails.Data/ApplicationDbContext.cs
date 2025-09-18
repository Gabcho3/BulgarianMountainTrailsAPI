using BulgarianMountainTrails.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrails.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Trail> Trails { get; set; }
        public DbSet<Hut> Huts { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
        public DbSet<River> Rivers { get; set; }
        public DbSet<Lake> Lakes { get; set; }
        public DbSet<Waterfall> Waterfalls { get; set; }
        public DbSet<Peak> Peaks { get; set; }
        public DbSet<Monastery> Monasteries { get; set; }
        public DbSet<Cave> Caves { get; set; }
        public DbSet<TrailHut> TrailHuts { get; set; }
        public DbSet<TrailPOI> TrailPOIs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PointOfInterest>().ToTable("PointsOfInterest");
            modelBuilder.Entity<River>().ToTable("Rivers");
            modelBuilder.Entity<Lake>().ToTable("Lakes");
            modelBuilder.Entity<Waterfall>().ToTable("Waterfalls");
            modelBuilder.Entity<Peak>().ToTable("Peaks");
            modelBuilder.Entity<Monastery>().ToTable("Monasteries");
            modelBuilder.Entity<Cave>().ToTable("Caves");

            modelBuilder.Entity<TrailPOI>()
                .HasKey(tp => new { tp.TrailId, tp.PointOfInterestId });

            modelBuilder.Entity<TrailPOI>()
                .HasOne(tp => tp.Trail)
                .WithMany(t => t.TrailPOIs)
                .HasForeignKey(tp => tp.TrailId);

            modelBuilder.Entity<TrailPOI>()
                .HasOne(tp => tp.PointOfInterest)
                .WithMany()
                .HasForeignKey(tp => tp.PointOfInterestId);

            modelBuilder.Entity<TrailHut>()
                .HasKey(th => new { th.TrailId, th.HutId });

            modelBuilder.Entity<Trail>()
                .HasMany(t => t.TrailHuts)
                .WithOne(th => th.Trail)
                .HasForeignKey(th => th.TrailId);

            modelBuilder.Entity<Hut>()
                .HasMany(h => h.TrailHuts)
                .WithOne(th => th.Hut)
                .HasForeignKey(th => th.HutId);
        }
    }
}
