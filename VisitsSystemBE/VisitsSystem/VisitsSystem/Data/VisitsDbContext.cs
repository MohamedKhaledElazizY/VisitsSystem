using Microsoft.EntityFrameworkCore;
using VisitsSystem.Models;

namespace VisitsSystem.Data
{
    public class VisitsDbContext: DbContext
    {
        public VisitsDbContext()
        {
        }

        public VisitsDbContext(DbContextOptions<VisitsDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> USERS { get; set; }
        public DbSet<Visit> VISITS { get; set; }
        public DbSet<Visitor> VISITORS { get; set; }
        public DbSet<PostponedVisit> POSTPONED_VISITS { get; set; } 
        public DbSet<Office> OFFICES { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visit>()
                .Property(b => b.State)
                .HasDefaultValue(0);
        }
    }
}