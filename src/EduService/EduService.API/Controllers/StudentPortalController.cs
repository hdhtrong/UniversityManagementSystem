using Asp.Versioning;
using AutoMapper;
using EduService.API.Models;
using EduService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduService.API.Controllers
{
    [Route("api/edu/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Student")]
    public class StudentPortalController : ControllerBase
    {
        private readonly IEduStudentPortalService _studentPortalService;
        private readonly IEduStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentPortalController(
            IEduStudentPortalService studentPortalService,
            IEduStudentService studentService,
            IMapper mapper)
        {
            _studentPortalService = studentPortalService;
            _studentService = studentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy thông tin học kỳ, bắt đầu từ năm học sinh viên nhập học
        /// </summary>
        [HttpGet("{studentId}/semesters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSemesters(string studentId)
        {
            var semesters = await _studentPortalService.GetSemesters(studentId);
            var dtos = _mapper.Map<IEnumerable<EduSemesterDto>>(semesters);
            return Ok(dtos);
        }

        /// <summary>
        /// Lấy thông tin thống kê
        /// </summary>
        [HttpGet("{studentId}/statistics")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatistics(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var statistics = await _studentPortalService.GetStatistics(studentId);
            return statistics == null ? NotFound() : Ok(statistics);
        }

        /// <summary>
        /// Lấy thông tin cơ bản
        /// </summary>
        [HttpGet("{studentId}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var student = await _studentPortalService.GetPersonalDetail(studentId);
            return student == null ? NotFound() : Ok(student);
        }

        /// <summary>
        /// Lấy chương trình đào tạo của sinh viên
        /// </summary>
        [HttpGet("{studentId}/curriculums")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurriculums(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var curriculums = await _studentPortalService.GetCurriculums(studentId);
            var dtos = _mapper.Map<IEnumerable<CurriculumSubjectDto>>(curriculums);
            return Ok(dtos);
        }

        /// <summary>
        /// Lấy môn tiên quyết
        /// </summary>
        [HttpGet("{studentId}/prerequisites")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPrerequisites(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var result = await _studentPortalService.GetStudentPrerequisites(studentId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thời khóa biểu của sinh viên theo học kỳ
        /// </summary>
        [HttpGet("{studentId}/schedule/{semesterCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetScheduleBySemester(string studentId, string semesterCode)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var schedules = await _studentPortalService.GetScheduleBySemester(studentId, semesterCode);
            return Ok(schedules);
        }

        /// <summary>
        /// Lấy điểm của sinh viên theo học kỳ
        /// </summary>
        [HttpGet("{studentId}/grades/{semesterCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGradesBySemester(string studentId, string semesterCode)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var grades = await _studentPortalService.GetGradesBySemester(studentId, semesterCode);
            return Ok(grades);
        }

        /// <summary>
        /// Lấy danh sách học phí
        /// </summary>
        [HttpGet("{studentId}/tuitions")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTuitions(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var tuitions = await _studentPortalService.GetTuitions(studentId);
            var dtos = _mapper.Map<IEnumerable<EduTuitionFeeDto>>(tuitions);
            return Ok(dtos);
        }

        /// <summary>
        /// Lấy danh sách hóa đơn
        /// </summary>
        [HttpGet("{studentId}/invoices")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInvoices(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var invoices = await _studentPortalService.GetInvoices(studentId);
            var dtos = _mapper.Map<IEnumerable<EduInvoiceDto>>(invoices);
            return Ok(dtos);
        }

        /// <summary>
        /// Cập nhật thông tin cá nhân sinh viên
        /// </summary>
        [HttpPut("{studentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Student")]
        public async Task<IActionResult> UpdatePersonalInfo(string studentId, [FromBody] EduUpdateStudentDetailDto dto)
        {
            if (dto == null) return BadRequest("Invalid student data");

            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var existingStudent = await _studentService.GetByStudentId(studentId);
            if (existingStudent == null) return NotFound();

            _mapper.Map(dto, existingStudent);

            var result = await _studentService.Update(existingStudent);
            return result
                ? Ok("Student Info updated successfully")
                : Problem("Failed to update student", statusCode: StatusCodes.Status500InternalServerError);
        }

        // 🔹 Helper methods
        private string? GetUserLoggedInCode()
        {
            return User.FindFirst("code")?.Value; //"code" in logged in user is studentId
        }

        private IActionResult? ValidateStudentAccess(string studentId)
        {
            var claimStudentId = GetUserLoggedInCode();
            if (claimStudentId == null)
                return Unauthorized("Token does not contain code (studentId)");

            if (!string.Equals(claimStudentId, studentId, StringComparison.OrdinalIgnoreCase))
                return Forbid("You are not allowed to access other student's info");

            return null;
        }
    }
}
