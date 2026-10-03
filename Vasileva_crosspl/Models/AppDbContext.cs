using Microsoft.EntityFrameworkCore;

namespace Vasileva_crosspl.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pipeline> Pipelines { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    }
}