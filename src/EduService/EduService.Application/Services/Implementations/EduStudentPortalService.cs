using EduService.Application.Dtos;
using EduService.Domain.Entities;
using EduService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EduService.Application.Services.Implementations
{
    public class EduStudentPortalService : IEduStudentPortalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduStudentPortalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EduSemester>> GetSemesters(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            var student = await _unitOfWork.StudentRepository
                .GetSingleByConditions(s => s.StudentID == studentId);

            if (student == null)
            {
                return Enumerable.Empty<EduSemester>();
            }

            if (string.IsNullOrWhiteSpace(studentId) || studentId.Length < 8)
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            // Lấy năm bắt đầu từ StudentID (ITCSIU22xxx -> "22" -> 2022)
            string century = DateTime.Now.Year.ToString().Substring(0, 2);
            int startYear = int.Parse(century + studentId.Substring(6, 2));
            int endYear = startYear + 7;

            // Query tất cả semester trong khoảng
            var semesters = await _unitOfWork.SemesterRepository
                            .GetMultiByConditions(s => !s.IsDeleted
                                && s.AcademicYear.StartDate.Value.Year >= startYear
                                && s.AcademicYear.StartDate.Value.Year <= endYear, new string[] { "AcademicYear" })
                            .OrderByDescending(s => s.AcademicYear.Order)
                            .ThenBy(s => s.SemesterOrder)
            .ToListAsync();

            return semesters;
        }

        public async Task<EduStudentDetailDto?> GetPersonalDetail(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            var s = await _unitOfWork.StudentRepository.GetSingleByConditions(s => s.StudentID.Equals(studentId), new string[] { "Class", "Class.Program", "Class.Instructor", "Class.Program.Major", "Class.Program.Major.Department" });
            if (s == null) return null;

            return new EduStudentDetailDto
            {
                Fullname = s.Fullname,
                DOB = s.DOB,
                Gender = s.Gender,
                Email = s.Email,
                Phone = s.Phone,
                StudentID = s.StudentID,
                IdentityNumber = s.IdentityNumber,
                Nationality = s.Nationality,
                Ethnicity = s.Ethnicity,
                Religion = s.Religion,
                ProvincePermanentAddress = s.ProvincePermanentAddress,   // Tỉnh/thành phố (thường trú)
                DistrictPermanentAddress = s.DistrictPermanentAddress,   // Quận/Huyện (thường trú)
                WardPermanentAddress = s.WardPermanentAddress,           // Xã/Phường (thường trú)
                HouseNumberPermanentAddress = s.HouseNumberPermanentAddress, // Số nhà (thường trú)
                TemporaryAddress = s.TemporaryAddress,   // Địa chỉ tạm trú
                EducationType = s.EducationType,         // Loại hình đào tạo

                MajorName = s.Class?.Program?.Major?.MajorName,
                MajorCode = s.Class?.Program?.Major?.MajorCode,
                ClassCode = s.Class?.ClassCode,
                ClassName = s.Class?.ClassName,
                ProgramName = s.Class?.Program?.ProgramName,
                ProgramCode = s.Class?.Program?.ProgramCode,
                TrainingFromYear = s.TrainingFromYear,   // Đào tạo từ năm
                DepartmentName = s.Class?.Program?.Major?.Department?.DepartmentName,

                InstructorName = s.Class?.Instructor?.Fullname,
                InstructorEmail = s.Class?.Instructor?.Email
            };
        }

        public async Task<EduStudentStatisticsDto> GetStatistics(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            var grades = await _unitOfWork.GradeRepository.GetMultiByConditions(
                g => g.Enrollment.Student.StudentID == studentId,
                [
                     "Enrollment.Student",
                    "Enrollment.Section.Subject"
                ]
            ).ToListAsync();

            if (grades == null || !grades.Any())
                return new EduStudentStatisticsDto();

            var totalCourses = grades.Count;
            var totalCredits = grades.Sum(g => g.Enrollment.Section.Subject.Credits);

            var passedCourses = grades.Count(g => g.Passed == true);
            var passedCredits = grades.Where(g => g.Passed == true).Sum(g => g.Enrollment.Section.Subject.Credits);

            var failedCourses = totalCourses - passedCourses;
            var failedCredits = totalCredits - passedCredits;

            return new EduStudentStatisticsDto
            {
                TotalCourses = totalCourses,
                TotalCredits = totalCredits,
                PassedCourses = passedCourses,
                PassedCredits = passedCredits,
                FailedCourses = failedCourses,
                FailedCredits = failedCredits
            };
        }

        public async Task<IEnumerable<EduCurriculum>> GetCurriculums(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            // Get Student Info
            var s = await _unitOfWork.StudentRepository.GetSingleByConditions(s => s.StudentID.Equals(studentId), new string[] { "Class" });
            if (s == null) 
                return null;
            // Get ProgramID
            var programId = s.Class.ProgramID;
            return await _unitOfWork.CurriculumRepository.GetMultiByConditions(c => c.ProgramID.Equals(programId), new string[] { "Subject", "Program" })
                .OrderBy(c => c.SemesterOrder)
                .OrderBy(c => c.Subject.SubjectCode)             
                .ToListAsync();
        }

        public async Task<IEnumerable<EduWeekScheduleDto>> GetScheduleBySemester(string studentId, string semesterCode)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            var enrollments = await _unitOfWork.EnrollmentRepository.GetMultiByConditions(
                e => e.Student.StudentID.ToString() == studentId
                     && e.Section.Semester.SemesterCode == semesterCode,
                [       
                        "Student",
                        "Section.Subject",
                        "Section.Instructor",
                        "Section.Semester",
                        "Section.Schedules.Room",
                        "Section.Schedules.ScheduleWeeks.Week"
                ]
            ).ToListAsync();

            if (enrollments == null || !enrollments.Any())
                return Enumerable.Empty<EduWeekScheduleDto>();

            // Flatten → mỗi Schedule x mỗi Week
            var flat = enrollments
                .SelectMany(e => e.Section.Schedules
                    .SelectMany(s => s.ScheduleWeeks.Select(sw => new
                    {
                        Week = sw.Week,
                        Item = new EduScheduleItemDto
                        {
                            SubjectCode = e.Section.Subject.SubjectCode,
                            SubjectName = e.Section.Subject.SubjectName,
                            SectionCode = e.Section.Code,
                            InstructorName = e.Section.Instructor?.Fullname ?? "",
                            RoomName = s.Room?.RoomName ?? "",
                            DayOfWeek = s.DayOfWeek,
                            StartPeriod = s.StartPeriod,
                            EndPeriod = s.EndPeriod
                        }
                    })));

            // Group theo tuần
            var grouped = flat
                .GroupBy(x => x.Week)
                .Select(g => new EduWeekScheduleDto
                {
                    WeekNumber = g.Key.WeekNumber,
                    WeekName = g.Key.WeekName,
                    Items = g.Select(x => x.Item)
                             .OrderBy(i => i.DayOfWeek)
                             .ThenBy(i => i.StartPeriod)
                             .ToList()
                })
                .OrderBy(w => w.WeekNumber)
                .ToList();

            return grouped;
        }

        public async Task<IEnumerable<EduSemesterGradeDto>> GetGradesBySemester(string studentId, string semesterCode)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            var grades = await _unitOfWork.GradeRepository.GetMultiByConditions(
                 g => g.Enrollment.Student.StudentID == studentId
                      && (semesterCode == "all" || g.Enrollment.Section.Semester.SemesterCode == semesterCode),
                 [
                    "Enrollment.Student",
                    "Enrollment.Section.Subject",
                    "Enrollment.Section.Semester"
                 ]
             ).ToListAsync();

            if (grades == null || !grades.Any())
                return Enumerable.Empty<EduSemesterGradeDto>();

            var grouped = grades
                .GroupBy(g => g.Enrollment.Section.Semester)
                .Select(g => new EduSemesterGradeDto
                {
                    SemesterCode = g.Key.SemesterCode,
                    SemesterName = g.Key.SemesterName,
                    SemesterOrder = g.Key.SemesterOrder.Value,
                    Items = g.Select(x => new EduGradeItemDto
                    {
                        SubjectCode = x.Enrollment.Section.Subject.SubjectCode,
                        SubjectName = x.Enrollment.Section.Subject.SubjectName,
                        SectionCode = x.Enrollment.Section.Code,
                        AssignmentScore = x.AssignmentScore,
                        MidtermScore = x.MidtermScore,
                        FinalExamScore = x.FinalExamScore,
                        Total100Score = x.Total100Score,
                        Total10Score = x.Total10Score,
                        Total4Score = x.Total4Score,
                        LetterGrade = x.LetterGrade,
                        Passed = x.Passed
                    }).OrderBy(i => i.SubjectCode).ToList()
                })
                .OrderBy(g => g.SemesterCode) 
                .ToList();

            return grouped;
        }

        public async Task<IEnumerable<EduSubjectWithPrereqDto>> GetStudentPrerequisites(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            // Lấy student
            var student = await _unitOfWork.StudentRepository
                .GetSingleByConditions(s => s.StudentID == studentId, ["Class.Program.Curriculums.Subject.Prerequisites.PrerequisiteSubject"]);

            if (student == null)
                return Enumerable.Empty<EduSubjectWithPrereqDto>();

            // Lấy chương trình đào tạo của student
            var program = student.Class?.Program;
            if (program == null || program.Curriculums == null)
                return Enumerable.Empty<EduSubjectWithPrereqDto>();

            // Map DTO
            var result = program.Curriculums
                .Where(c => !c.IsDeleted && c.Subject != null)
                .Select(c => new EduSubjectWithPrereqDto
                {
                    SubjectCode = c.Subject!.SubjectCode ?? string.Empty,
                    SubjectName = c.Subject!.SubjectName ?? string.Empty,
                    Credits = c.Subject.Credits,
                    SemesterOrder = c.SemesterOrder,
                    Compulsory = c.Compulsory,
                    Prerequisites = c.Subject.Prerequisites?
                        .Select(p => new EduPrerequisiteDto
                        {
                            SubjectCode = p.PrerequisiteSubject!.SubjectCode ?? string.Empty,
                            SubjectName = p.PrerequisiteSubject!.SubjectName ?? string.Empty,
                            Order = p.Order,
                            Type = p.Type,
                            Note = p.Note
                        }).ToList() ?? new List<EduPrerequisiteDto>()
                })
                .OrderBy(c => c.SemesterOrder) // sắp xếp theo học kỳ
                .ThenBy(c => c.SubjectCode)
                .ToList();

            return result;
        }

        public async Task<IEnumerable<EduTuitionFee>> GetTuitions(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            var tuitions = await _unitOfWork.TuitionFeeRepository
                .GetMultiByConditions(t => !t.IsDeleted && t.Student!.StudentID == studentId,
                                      ["Student", "Semester"])
                .OrderByDescending(t => t.Semester.AcademicYear.Order)
                .ThenBy(t => t.Semester.SemesterOrder)
                .ToListAsync();

            return tuitions;
        }

        public async Task<IEnumerable<EduInvoice>> GetInvoices(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Invalid StudentID", nameof(studentId));

            var invoices = await _unitOfWork.InvoiceRepository
                .GetMultiByConditions(i => !i.IsDeleted && i.StudentID == studentId,
                                      ["TuitionFee"])
                .OrderByDescending(i => i.PaymentDate)
                .ToListAsync();

            return invoices;
        }
    }
}
