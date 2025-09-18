using Asp.Versioning;
using EduService.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EduService.Application.Services;
using EduService.Domain.Entities;
using Shared.SharedKernel.Models;
using SharedKernel.Models;

namespace EduService.API.Controllers.v2
{
    [Route("api/edu/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IEduStudentService _studentService;
        private readonly IEduClassService _eduClassService;
        private readonly IMapper _mapper;

        public StudentsController(IEduStudentService studentService, IEduClassService eduClassService, IMapper mapper)
        {
            _studentService = studentService;
            _eduClassService = eduClassService;
            _mapper = mapper;
        }

        [HttpPost("Import")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> ImportStudents(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponse("File is empty or not provided"));

            try
            {
                var result = await _studentService.ImportStudentsAsync(file);
                return Ok(new ApiResponse("Import successful", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse($"Import failed: {ex.Message}"));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAll();
            var result = _mapper.Map<IEnumerable<EduStudentDto>>(students);
            return Ok(new ApiResponse("Fetched students successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _studentService.GetById(id);
            if (student == null)
                return NotFound(new ApiResponse("Student not found"));

            var dto = _mapper.Map<EduStudentDto>(student);
            return Ok(new ApiResponse("Fetched student successfully", dto));
        }

        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var student = await _studentService.GetStudentDetailAsync(id);
            if (student == null)
                return NotFound(new ApiResponse("Student not found"));

            return Ok(new ApiResponse("Fetched student detail successfully", student));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduStudentDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Student data is required"));

            if (dto.ClassID.HasValue)
            {
                var checkClass = await _eduClassService.GetById(dto.ClassID.Value);
                if (checkClass == null)
                    return BadRequest(new ApiResponse("ClassID does not exist"));
            }

            if (!string.IsNullOrEmpty(dto.StudentID))
            {
                var checkStudentId = await _studentService.GetByStudentId(dto.StudentID);
                if (checkStudentId != null)
                    return BadRequest(new ApiResponse($"StudentID already exists: {dto.StudentID}"));
            }

            var entity = _mapper.Map<EduStudent>(dto);
            var result = await _studentService.Create(entity);
            return result
                ? Ok(new ApiResponse("Student created successfully"))
                : StatusCode(500, new ApiResponse("Failed to create student"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EduStudentDto dto)
        {
            if (dto == null || id != dto.ID)
                return BadRequest(new ApiResponse("Invalid student data"));

            var existingStudent = await _studentService.GetById(id);
            if (existingStudent == null)
                return NotFound(new ApiResponse("Student not found"));

            var entity = _mapper.Map<EduStudent>(dto);
            var result = await _studentService.Update(entity);
            return result
                ? Ok(new ApiResponse("Student updated successfully"))
                : StatusCode(500, new ApiResponse("Failed to update student"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existingStudent = await _studentService.GetById(guidId);
            if (existingStudent == null)
                return NotFound(new ApiResponse("Student not found"));

            var result = await _studentService.Delete(guidId);
            return result
                ? Ok(new ApiResponse("Student deleted successfully"))
                : StatusCode(500, new ApiResponse("Failed to delete student"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var students = _studentService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduStudentDto>>(students);

            return Ok(new ApiResponse("Fetched students successfully", new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            }));
        }
    }
}
