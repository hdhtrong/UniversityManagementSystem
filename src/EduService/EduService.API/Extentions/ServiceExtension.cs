
using EduService.Application.Services;
using EduService.Application.Services.Implementations;
using EduService.Infrastructure;
using EduService.Infrastructure.Interfaces;
using EduService.Infrastructure.Repositories;
using EduService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduService.API.Extentions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            var connectionName = env.IsEnvironment("Development") ? "DockerEduDBConnection" : "LocalEduDBConnection";
            services.AddDbContext<EduDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionName));
            });
            services.AddScoped<Func<EduDbContext>>((provider) => () => provider.GetService<EduDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // ===== Academic =====
            services.AddScoped<IEduDepartmentRepository, EduDepartmentRepository>();
            services.AddScoped<IEduInstructorRepository, EduInstructorRepository>();
            services.AddScoped<IEduMajorRepository, EduMajorRepository>();
            services.AddScoped<IEduProgramRepository, EduProgramRepository>();
            services.AddScoped<IEduSubjectRepository, EduSubjectRepository>();
            services.AddScoped<IEduSubjectPrerequisiteRepository, EduSubjectPrerequisiteRepository>();
            services.AddScoped<IEduCurriculumRepository, EduCurriculumRepository>();

            // ===== Classes & Students =====
            services.AddScoped<IEduClassRepository, EduClassRepository>();
            services.AddScoped<IEduStudentRepository, EduStudentRepository>();
            services.AddScoped<IEduTuitionFeeRepository, EduTuitionFeeRepository>();
            services.AddScoped<IEduInvoiceRepository, EduInvoiceRepository>();

            // ===== Scheduling =====
            services.AddScoped<IEduScheduleRepository, EduScheduleRepository>();
            services.AddScoped<IEduScheduleWeekRepository, EduScheduleWeekRepository>();
            services.AddScoped<IEduRoomRepository, EduRoomRepository>();
            services.AddScoped<IEduAcademicYearRepository, EduAcademicYearRepository>();
            services.AddScoped<IEduSemesterRepository, EduSemesterRepository>();
            services.AddScoped<IEduWeekRepository, EduWeekRepository>();
            services.AddScoped<IEduPeriodRepository, EduPeriodRepository>();

            // ===== Course Sections =====
            services.AddScoped<IEduCourseSectionRepository, EduCourseSectionRepository>();

            // ===== Enrollment & Assessment =====
            services.AddScoped<IEduEnrollmentRepository, EduEnrollmentRepository>();
            services.AddScoped<IEduGradeRepository, EduGradeRepository>();
            services.AddScoped<IEduExamSessionRepository, EduExamSessionRepository>();
            services.AddScoped<IEduExamRepository, EduExamRepository>();
            services.AddScoped<IEduAttendanceRepository, EduAttendanceRepository>();

            // ===== Unit of Work =====
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // ===== Academic =====
            services.AddScoped<IEduDepartmentService, EduDepartmentService>();
            services.AddScoped<IEduInstructorService, EduInstructorService>();
            services.AddScoped<IEduMajorService, EduMajorService>();
            services.AddScoped<IEduProgramService, EduProgramService>();
            services.AddScoped<IEduSubjectService, EduSubjectService>();
            services.AddScoped<IEduSubjectPrerequisiteService, EduSubjectPrerequisiteService>();
            services.AddScoped<IEduCurriculumService, EduCurriculumService>();

            // ===== Classes & Students =====
            services.AddScoped<IEduClassService, EduClassService>();
            services.AddScoped<IEduStudentService, EduStudentService>();
            services.AddScoped<IEduStudentPortalService, EduStudentPortalService>();
            services.AddScoped<IEduTuitionFeeService, EduTuitionFeeService>();
            services.AddScoped<IEduInvoiceService, EduInvoiceService>();

            // ===== Scheduling =====
            services.AddScoped<IEduScheduleService, EduScheduleService>();
            services.AddScoped<IEduScheduleWeekService, EduScheduleWeekService>();
            services.AddScoped<IEduRoomService, EduRoomService>();
            services.AddScoped<IEduAcademicYearService, EduAcademicYearService>();
            services.AddScoped<IEduSemesterService, EduSemesterService>();
            services.AddScoped<IEduWeekService, EduWeekService>();
            services.AddScoped<IEduPeriodService, EduPeriodService>();

            // ===== Course Sections =====
            services.AddScoped<IEduCourseSectionService, EduCourseSectionService>();

            // ===== Enrollment & Assessment =====
            services.AddScoped<IEduEnrollmentService, EduEnrollmentService>();
            services.AddScoped<IEduGradeService, EduGradeService>();
            services.AddScoped<IEduExamSessionService, EduExamSessionService>();
            services.AddScoped<IEduExamService, EduExamService>();
            services.AddScoped<IEduAttendanceService, EduAttendanceService>();

            return services;
        }
    }
}
