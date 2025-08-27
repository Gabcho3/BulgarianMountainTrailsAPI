using BulgarianMountainTrailsAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BulgarianMountainTrailsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Trail> Trails { get; set; }
        public DbSet<MountainHut> Huts { get; set; }
        public DbSet<TrailHut> TrailHuts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrailHut>()
                .HasKey(th => new { th.TrailId, th.HutId });

            modelBuilder.Entity<Trail>()
                .HasMany(t => t.TrailHuts)
                .WithOne(th => th.Trail)
                .HasForeignKey(th => th.TrailId);

            modelBuilder.Entity<MountainHut>()
                .HasMany(h => h.TrailHuts)
                .WithOne(th => th.Hut)
                .HasForeignKey(th => th.HutId);
        }

    }
}
