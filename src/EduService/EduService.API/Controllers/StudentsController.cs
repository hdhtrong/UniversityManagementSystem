using Asp.Versioning;
using EduService.API.Models;
using EduService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel.Models;
using AutoMapper;
using EduService.Application.Services;

namespace EduService.API.Controllers
{
    [Route("api/edu/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
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
                return BadRequest("File is empty or not provided");

            try
            {
                var result = await _studentService.ImportStudentsAsync(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Import failed: {ex.Message}");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAll();
            var result = _mapper.Map<IEnumerable<EduStudentDto>>(students);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _studentService.GetById(id);
            if (student == null)
                return NotFound();

            var dto = _mapper.Map<EduStudentDto>(student);
            return Ok(dto);
        }

        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var student = await _studentService.GetStudentDetailAsync(id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduStudentDto dto)
        {
            if (dto == null)
                return BadRequest("Student data is required");

            if (dto.ClassID.HasValue)
            {
                var checkClass = await _eduClassService.GetById(dto.ClassID.Value);
                if (checkClass == null)
                    return BadRequest("ClassID does not exist");
            }

            if(!string.IsNullOrEmpty(dto.StudentID))
            {
                var checkStudentId = _studentService.GetByStudentId(dto.StudentID);
                if(checkStudentId != null)
                    return BadRequest("StudentID already exists, " + dto.StudentID);
            }

            var entity = _mapper.Map<EduStudent>(dto);
            var result = await _studentService.Create(entity);
            if (result)
                return Ok("Student created successfully");
            return StatusCode(500, "Failed to create student");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EduStudentDto dto)
        {
            if (dto == null || id != dto.ID)
                return BadRequest("Invalid student data");

            var existingStudent = await _studentService.GetById(id);
            if (existingStudent == null)
                return NotFound();

            var entity = _mapper.Map<EduStudent>(dto);
            var result = await _studentService.Update(entity);
            if (result)
                return Ok("Student updated successfully");

            return StatusCode(500, "Failed to update student");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existingStudent = await _studentService.GetById(guidId);
            if (existingStudent == null)
                return NotFound();

            var result = await _studentService.Delete(guidId);
            if (result)
                return Ok("Student deleted successfully");

            return StatusCode(500, "Failed to delete student");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var students = _studentService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduStudentDto>>(students);

            return Ok(new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            });
        }
    }
}
