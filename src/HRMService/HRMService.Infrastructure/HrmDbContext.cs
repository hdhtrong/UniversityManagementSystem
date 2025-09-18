using HRMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace HRMService.Infrastructure
{
    public class HrmDbContext : DbContext
    {
        public HrmDbContext(DbContextOptions<HrmDbContext> options)
            : base(options)
        {

        }

        public DbSet<HrmDepartment> Departments { get; set; }
        public DbSet<HrmEmployee> Employees { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditableEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.Entity is AuditableEntity entity)
                {
                    var now = DateTime.UtcNow;
                    var user = "System";
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        if (string.IsNullOrEmpty(entity.CreatedBy))
                            entity.CreatedBy = user;
                        entity.IsDeleted = false;
                    }
                    entity.UpdatedAt = now;
                    if (string.IsNullOrEmpty(entity.UpdatedBy))
                        entity.UpdatedBy = user;
                }
            }
            return base.SaveChanges();
        }

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

            // Apply global filter cho tất cả entity kế thừa từ AuditableEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(HrmDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
