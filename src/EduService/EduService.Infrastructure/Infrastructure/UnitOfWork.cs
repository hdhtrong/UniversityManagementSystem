using EduService.Infrastructure.Interfaces;
using EduService.Infrastructure.Repositories;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EduDbContext _dbContext;

        // ===== Academic structure =====
        public IEduDepartmentRepository DepartmentRepository { get; }
        public IEduInstructorRepository InstructorRepository { get; }
        public IEduMajorRepository MajorRepository { get; }
        public IEduProgramRepository ProgramRepository { get; }
        public IEduSubjectRepository SubjectRepository { get; }
        public IEduSubjectPrerequisiteRepository SubjectPrerequisiteRepository { get; }
        public IEduCurriculumRepository CurriculumRepository { get; }

        // ===== Classes & Students =====
        public IEduClassRepository ClassRepository { get; }
        public IEduStudentRepository StudentRepository { get; }
        public  IEduTuitionFeeRepository TuitionFeeRepository { get; }
        public IEduInvoiceRepository InvoiceRepository { get; }

        // ===== Scheduling =====
        public IEduScheduleRepository ScheduleRepository { get; }
        public IEduScheduleWeekRepository ScheduleWeekRepository { get; }
        public IEduRoomRepository RoomRepository { get; }
        public IEduAcademicYearRepository AcademicYearRepository { get; }
        public IEduSemesterRepository SemesterRepository { get; }
        public IEduWeekRepository WeekRepository { get; }
        public IEduPeriodRepository PeriodRepository { get; }

        // ===== Course Sections =====
        public IEduCourseSectionRepository CourseSectionRepository { get; }

        // ===== Enrollment & Assessment =====
        public IEduEnrollmentRepository EnrollmentRepository { get; }
        public IEduGradeRepository GradeRepository { get; }
        public IEduExamSessionRepository ExamSessionRepository { get; }
        public IEduExamRepository ExamRepository { get; }
        public IEduAttendanceRepository AttendanceRepository { get; }

        public UnitOfWork(
            EduDbContext dbContext,

            // Academic
            IEduDepartmentRepository departmentRepository,
            IEduInstructorRepository instructorRepository,
            IEduMajorRepository majorRepository,
            IEduProgramRepository programRepository,
            IEduSubjectRepository subjectRepository,
            IEduSubjectPrerequisiteRepository subjectPrerequisiteRepository,
            IEduCurriculumRepository curriculumRepository,

            // Classes & Students
            IEduClassRepository classRepository,
            IEduStudentRepository studentRepository,
            IEduTuitionFeeRepository tuitionFeeRepository,
            IEduInvoiceRepository invoiceRepository,

            // Scheduling
            IEduScheduleRepository scheduleRepository,
            IEduScheduleWeekRepository scheduleWeekRepository,
            IEduRoomRepository roomRepository,
            IEduAcademicYearRepository academicYearRepository,
            IEduSemesterRepository semesterRepository,
            IEduWeekRepository weekRepository,
            IEduPeriodRepository periodRepository,

            // Course Sections
            IEduCourseSectionRepository courseSectionRepository,

            // Enrollment & Assessment
            IEduEnrollmentRepository enrollmentRepository,
            IEduGradeRepository gradeRepository,
            IEduExamSessionRepository examSessionRepository,
            IEduExamRepository examRepository,
            IEduAttendanceRepository attendanceRepository
        )
        {
            _dbContext = dbContext;

            // Academic
            DepartmentRepository = departmentRepository;
            InstructorRepository = instructorRepository;
            MajorRepository = majorRepository;
            ProgramRepository = programRepository;
            SubjectRepository = subjectRepository;
            SubjectPrerequisiteRepository = subjectPrerequisiteRepository;
            CurriculumRepository = curriculumRepository;

            // Classes & Students
            ClassRepository = classRepository;
            StudentRepository = studentRepository;
            TuitionFeeRepository = tuitionFeeRepository;
            InvoiceRepository = invoiceRepository;

            // Scheduling
            ScheduleRepository = scheduleRepository;
            ScheduleWeekRepository = scheduleWeekRepository;
            RoomRepository = roomRepository;
            AcademicYearRepository = academicYearRepository;
            SemesterRepository = semesterRepository;
            WeekRepository = weekRepository;
            PeriodRepository = periodRepository;

            // Course Sections
            CourseSectionRepository = courseSectionRepository;

            // Enrollment & Assessment
            EnrollmentRepository = enrollmentRepository;
            GradeRepository = gradeRepository;
            ExamSessionRepository = examSessionRepository;
            ExamRepository = examRepository;
            AttendanceRepository = attendanceRepository;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
