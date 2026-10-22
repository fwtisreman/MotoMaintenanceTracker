using Microsoft.EntityFrameworkCore;
using MotoMaintenanceTracker.Models;




namespace MotoMaintenanceTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }

        public DbSet<MaintenanceTask> MaintenanceTasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceRecord>()
                .Property(s => s.Cost)
                .HasPrecision(10, 2);
        }
    }
}
