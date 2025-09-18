using Asp.Versioning;
using AutoMapper;
using EduService.API.Models;
using EduService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Models;

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

        [HttpGet("{studentId}/semesters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSemesters(string studentId)
        {
            var semesters = await _studentPortalService.GetSemesters(studentId);
            var dtos = _mapper.Map<IEnumerable<EduSemesterDto>>(semesters);
            return Ok(new ApiResponse("Fetched semesters successfully", dtos));
        }

        [HttpGet("{studentId}/statistics")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatistics(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var statistics = await _studentPortalService.GetStatistics(studentId);
            return statistics == null
                ? NotFound(new ApiResponse("No statistics found"))
                : Ok(new ApiResponse("Fetched statistics successfully", statistics));
        }

        [HttpGet("{studentId}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var student = await _studentPortalService.GetPersonalDetail(studentId);
            return student == null
                ? NotFound(new ApiResponse("Student not found"))
                : Ok(new ApiResponse("Fetched student detail successfully", student));
        }

        [HttpGet("{studentId}/curriculums")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurriculums(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var curriculums = await _studentPortalService.GetCurriculums(studentId);
            var dtos = _mapper.Map<IEnumerable<CurriculumSubjectDto>>(curriculums);
            return Ok(new ApiResponse("Fetched curriculums successfully", dtos));
        }

        [HttpGet("{studentId}/prerequisites")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPrerequisites(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var result = await _studentPortalService.GetStudentPrerequisites(studentId);
            return Ok(new ApiResponse("Fetched prerequisites successfully", result));
        }

        [HttpGet("{studentId}/schedule/{semesterCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetScheduleBySemester(string studentId, string semesterCode)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var schedules = await _studentPortalService.GetScheduleBySemester(studentId, semesterCode);
            return Ok(new ApiResponse("Fetched schedule successfully", schedules));
        }

        [HttpGet("{studentId}/grades/{semesterCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGradesBySemester(string studentId, string semesterCode)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var grades = await _studentPortalService.GetGradesBySemester(studentId, semesterCode);
            return Ok(new ApiResponse("Fetched grades successfully", grades));
        }

        [HttpGet("{studentId}/tuitions")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTuitions(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var tuitions = await _studentPortalService.GetTuitions(studentId);
            var dtos = _mapper.Map<IEnumerable<EduTuitionFeeDto>>(tuitions);
            return Ok(new ApiResponse("Fetched tuitions successfully", dtos));
        }

        [HttpGet("{studentId}/invoices")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInvoices(string studentId)
        {
            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var invoices = await _studentPortalService.GetInvoices(studentId);
            var dtos = _mapper.Map<IEnumerable<EduInvoiceDto>>(invoices);
            return Ok(new ApiResponse("Fetched invoices successfully", dtos));
        }

        [HttpPut("{studentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Student")]
        public async Task<IActionResult> UpdatePersonalInfo(string studentId, [FromBody] EduUpdateStudentDetailDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Invalid student data"));

            var validationResult = ValidateStudentAccess(studentId);
            if (validationResult != null) return validationResult;

            var existingStudent = await _studentService.GetByStudentId(studentId);
            if (existingStudent == null)
                return NotFound(new ApiResponse("Student not found"));

            _mapper.Map(dto, existingStudent);

            var result = await _studentService.Update(existingStudent);
            return result
                ? Ok(new ApiResponse("Student info updated successfully"))
                : StatusCode(500, new ApiResponse("Failed to update student"));
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
                return Unauthorized(new ApiResponse("Token does not contain code (studentId)"));

            if (!string.Equals(claimStudentId, studentId, StringComparison.OrdinalIgnoreCase))
                return Unauthorized(new ApiResponse("You are not allowed to access other student's info"));

            return null;
        }
    }
}
