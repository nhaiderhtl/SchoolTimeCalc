using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; } = null!;
        public DbSet<WebUntisData> WebUntisData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebUntisData>()
                .HasOne(w => w.ApplicationUser)
                .WithOne(u => u.WebUntisData)
                .HasForeignKey<WebUntisData>(w => w.ApplicationUserId);

            modelBuilder.Entity<WebUntisData>()
                .Property(w => w.SubjectsJson)
                .HasColumnType("jsonb");

            modelBuilder.Entity<WebUntisData>()
                .Property(w => w.TeachersJson)
                .HasColumnType("jsonb");

            modelBuilder.Entity<WebUntisData>()
                .Property(w => w.RoomsJson)
                .HasColumnType("jsonb");

            modelBuilder.Entity<WebUntisData>()
                .Property(w => w.LessonsJson)
                .HasColumnType("jsonb");
        }
    }
}
