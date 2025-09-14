using EduService.Infrastructure.Interfaces;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        // ===== Academic structure =====
        IEduDepartmentRepository DepartmentRepository { get; }
        IEduInstructorRepository InstructorRepository { get; }
        IEduMajorRepository MajorRepository { get; }
        IEduProgramRepository ProgramRepository { get; }
        IEduSubjectRepository SubjectRepository { get; }
        IEduSubjectPrerequisiteRepository SubjectPrerequisiteRepository { get; }
        IEduCurriculumRepository CurriculumRepository { get; }

        // ===== Classes & Students =====
        IEduClassRepository ClassRepository { get; }
        IEduStudentRepository StudentRepository { get; }
        IEduTuitionFeeRepository TuitionFeeRepository { get; }
        IEduInvoiceRepository InvoiceRepository { get; }

        // ===== Scheduling =====
        IEduScheduleRepository ScheduleRepository { get; }
        IEduScheduleWeekRepository ScheduleWeekRepository { get; }
        IEduRoomRepository RoomRepository { get; }
        IEduAcademicYearRepository AcademicYearRepository { get; }
        IEduSemesterRepository SemesterRepository { get; }
        IEduWeekRepository WeekRepository { get; }
        IEduPeriodRepository PeriodRepository { get; }

        // ===== Course Sections =====
        IEduCourseSectionRepository CourseSectionRepository { get; }

        // ===== Enrollment & Assessment =====
        IEduEnrollmentRepository EnrollmentRepository { get; }
        IEduGradeRepository GradeRepository { get; }
        IEduExamSessionRepository ExamSessionRepository { get; }
        IEduExamRepository ExamRepository { get; }
        IEduAttendanceRepository AttendanceRepository { get; }

        // ===== Transaction =====
        int Save();
    }
}
