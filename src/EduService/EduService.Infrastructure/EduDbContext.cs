using EduService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace EduService.Infrastructure
{
    public class EduDbContext : DbContext
    {
        public EduDbContext(DbContextOptions<EduDbContext> options)
            : base(options)
        {

        }

        #region Organization (Tổ chức khoa/bộ môn)
        public DbSet<EduDepartment> Departments { get; set; }
        public DbSet<EduInstructor> Instructors { get; set; }
        public DbSet<EduMajor> Majors { get; set; }
        public DbSet<EduProgram> Programs { get; set; }
        #endregion

        #region Subjects & Curriculum (Môn học & Chương trình đào tạo)
        public DbSet<EduSubject> Subjects { get; set; }
        public DbSet<EduSubjectPrerequisite> SubjectPrerequisites { get; set; }
        public DbSet<EduCurriculum> Curriculums { get; set; }
        public DbSet<EduCourseSection> CourseSections { get; set; }   // Lớp học phần
        #endregion

        #region Students & Classes (Sinh viên & Lớp hành chính)
        public DbSet<EduClass> Classes { get; set; }
        public DbSet<EduStudent> Students { get; set; }
        public DbSet<EduTuitionFee> TuitionFees { get; set; }
        public DbSet<EduInvoice> Invoices { get; set; }
        #endregion

        #region Enrollment & Assessment (Đăng ký & Đánh giá kết quả học tập)
        public DbSet<EduEnrollment> Enrollments { get; set; }
        public DbSet<EduGrade> Grades { get; set; }
        public DbSet<EduExam> Exams { get; set; }
        public DbSet<EduAttendance> Attendances { get; set; }
        #endregion

        #region Scheduling & Facilities (Lịch học & Cơ sở vật chất)
        public DbSet<EduSchedule> Schedules { get; set; }
        public DbSet<EduScheduleWeek> ScheduleWeeks { get; set; }     // Bảng nối Schedule - Week
        public DbSet<EduRoom> Rooms { get; set; }
        #endregion

        #region Academic Calendar (Niên khóa - Học kỳ - Tuần - Tiết học)
        public DbSet<EduAcademicYear> AcademicYears { get; set; }
        public DbSet<EduSemester> Semesters { get; set; }
        public DbSet<EduWeek> Weeks { get; set; }
        public DbSet<EduPeriod> Periods { get; set; }
        #endregion

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

            // Composite key cho SubjectPrerequisites
            modelBuilder.Entity<EduSubjectPrerequisite>()
                .HasOne(sp => sp.Subject) // Môn học chính
                .WithMany(s => s.Prerequisites) // Danh sách môn tiên quyết cho môn này
                .HasForeignKey(sp => sp.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EduSubjectPrerequisite>()
                .HasOne(sp => sp.PrerequisiteSubject) // Môn tiên quyết
                .WithMany(s => s.IsPrerequisiteFor) // Danh sách môn mà nó là tiên quyết
                .HasForeignKey(sp => sp.PrerequisiteSubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite key cho Curriculums
            modelBuilder.Entity<EduCurriculum>()
                .HasKey(c => new { c.ProgramID, c.SubjectID });

            // Composite key cho bảng nối ScheduleWeeks
            modelBuilder.Entity<EduScheduleWeek>()
                .HasKey(sw => new { sw.ScheduleID, sw.WeekID });

            // Apply global filter cho tất cả entity kế thừa từ AuditableEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(EduDbContext)
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
