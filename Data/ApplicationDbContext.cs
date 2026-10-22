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


        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceRecord>()
                .Property(s => s.Cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MaintenanceTask>()
            .HasOne(t => t.Motorcycle)
            .WithMany()
            .HasForeignKey(t => t.MotorcycleId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne<MaintenanceTask>()
                .WithMany()
                .HasForeignKey(m => m.MaintenanceTaskId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
