using HRMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMService.Infrastructure
{
    public class HrmDbContext : DbContext
    {
        public HrmDbContext(DbContextOptions<HrmDbContext> options)
            : base(options)
        {
        }

        public DbSet<HrmDepartment> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configuration using Fluent API
            modelBuilder.Entity<HrmDepartment>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(e => e.ID);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.HasIndex(e => e.ShortName).IsUnique();             
            });
        }
    }
}
