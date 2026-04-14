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
    }
}
