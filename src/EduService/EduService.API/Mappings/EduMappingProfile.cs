using EduService.API.Models;
using EduService.Domain.Entities;
using AutoMapper;
using EduService.Application.Dtos;

namespace EduService.API.Mappings
{
    public class EduMappingProfile : Profile
    {
        public EduMappingProfile()
        {
            // ============================
            // STUDENT
            // ============================
            CreateMap<EduStudentImportDto, EduStudent>()
            .ForMember(dest => dest.Fullname,
                       opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}".Trim()))
            .ForMember(dest => dest.MiddleName,
                       opt => opt.MapFrom(src => src.LastName));

            CreateMap<EduStudent, EduStudentDto>()
                // Nếu ClassID null thì gán Guid.Empty để tránh lỗi
                .ForMember(dest => dest.ClassID, opt => opt.MapFrom(src => src.ClassID ?? Guid.Empty))
                // Giữ nguyên DOB (có thể custom format tại đây nếu cần)
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.DOB))
                .ReverseMap()
                // Không cho phép map ngược ID để tránh ghi đè khóa chính
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                // Bỏ qua navigation property
                .ForMember(dest => dest.Class, opt => opt.Ignore());

            CreateMap<EduStudentDto, EduStudent>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // tránh overwrite ID
                .ForMember(dest => dest.Class, opt => opt.Ignore()) // tránh vòng lặp navigation
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.DOB ?? string.Empty)) // fallback tránh null
                .ForMember(dest => dest.ClassID, opt => opt.MapFrom(src => (Guid?)src.ClassID)); // cho phép nullable

            CreateMap<EduUpdateStudentDetailDto, EduStudent>();

            // ============================
            // SCHEDULE
            // ============================
            CreateMap<EduSchedule, EduScheduleDto>()
                .ForMember(dest => dest.RoomName,
                           opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomName : string.Empty));

            // ============================
            // COURSE SECTION
            // ============================
            CreateMap<EduCourseSection, EduCourseSectionDto>()
                .ForMember(dest => dest.SubjectID, opt => opt.MapFrom(src => src.SubjectID ?? Guid.Empty))
                .ForMember(dest => dest.SemesterID, opt => opt.MapFrom(src => src.SemesterID ?? Guid.Empty))
                .ForMember(dest => dest.InstructorID, opt => opt.MapFrom(src => src.InstructorID ?? Guid.Empty))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject != null ? src.Subject.SubjectName : string.Empty))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester != null ? src.Semester.SemesterName : string.Empty))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.Fullname : string.Empty))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules))
                .ReverseMap()
                // Ignore navigation để tránh vòng lặp hoặc tracking lỗi EF
                .ForMember(dest => dest.Subject, opt => opt.Ignore())
                .ForMember(dest => dest.Semester, opt => opt.Ignore())
                .ForMember(dest => dest.Instructor, opt => opt.Ignore())
                .ForMember(dest => dest.Schedules, opt => opt.Ignore())
                .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore());

            // ============================
            // ENROLLMENT
            // ============================
            CreateMap<EduEnrollment, EduEnrollmentDto>()
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.StudentID ?? Guid.Empty))
                .ForMember(dest => dest.SectionID, opt => opt.MapFrom(src => src.SectionID ?? Guid.Empty))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Fullname : string.Empty))
                .ForMember(dest => dest.SectionCode, opt => opt.MapFrom(src => src.Section != null ? src.Section.Code : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Section, opt => opt.Ignore())
                .ForMember(dest => dest.Grades, opt => opt.Ignore())
                .ForMember(dest => dest.Attendances, opt => opt.Ignore());

            // ============================
            // GRADE
            // ============================
            CreateMap<EduGrade, EduGradeDto>()
                .ForMember(dest => dest.EnrollmentID, opt => opt.MapFrom(src => src.EnrollmentID ?? Guid.Empty))
                .ForMember(dest => dest.StudentName,
                           opt => opt.MapFrom(src => src.Enrollment != null && src.Enrollment.Student != null
                                ? src.Enrollment.Student.Fullname
                                : string.Empty))
                .ForMember(dest => dest.SectionCode,
                           opt => opt.MapFrom(src => src.Enrollment != null && src.Enrollment.Section != null
                                ? src.Enrollment.Section.Code
                                : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Enrollment, opt => opt.Ignore());

            // ============================
            // BASIC ENTITIES (ReverseMap là đủ)
            // ============================
            CreateMap<EduTuitionFee, EduTuitionFeeDto>().ReverseMap();
            CreateMap<EduInvoice, EduInvoiceDto>().ReverseMap();
            CreateMap<EduDepartment, EduDepartmentDto>().ReverseMap();
            CreateMap<EduInstructor, EduInstructorDto>().ReverseMap();
            CreateMap<EduMajor, EduMajorDto>().ReverseMap();
            CreateMap<EduProgram, EduProgramDto>().ReverseMap();
            CreateMap<EduCurriculum, EduCurriculumDto>().ReverseMap();
            CreateMap<EduSubject, EduSubjectDto>().ReverseMap();
            CreateMap<EduSubjectPrerequisite, EduSubjectPrerequisiteDto>().ReverseMap();
            CreateMap<EduClass, EduClassDto>().ReverseMap();
            CreateMap<EduAcademicYear, EduAcademicYearDto>().ReverseMap();
            CreateMap<EduSemester, EduSemesterDto>().ReverseMap();
            CreateMap<EduWeek, EduWeekDto>().ReverseMap();
            CreateMap<EduPeriod, EduPeriodDto>().ReverseMap();
            CreateMap<EduRoom, EduRoomDto>().ReverseMap();

            // ============================
            // CURRICULUM SUBJECT VIEW (custom projection)
            // ============================
            CreateMap<EduCurriculum, CurriculumSubjectDto>()
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectID))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Subject.SubjectCode))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => src.Subject.Credits))
                .ForMember(dest => dest.ProgramCode, opt => opt.MapFrom(src => src.Program.ProgramCode))
                .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.Program.ProgramName))
                .ForMember(dest => dest.SemesterOrder, opt => opt.MapFrom(src => src.SemesterOrder))
                .ForMember(dest => dest.Compulsory, opt => opt.MapFrom(src => src.Compulsory));

            // ============================
            // ACADEMIC YEAR SPECIAL CASE (ignore Semesters to tránh loop)
            // ============================
            CreateMap<EduAcademicYear, EduAcademicYearDto>()
                .ForMember(dest => dest.Semesters, opt => opt.Ignore())
                .ReverseMap();

        }
    }
}
