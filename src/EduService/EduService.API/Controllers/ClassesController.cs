using Asp.Versioning;
using AutoMapper;
using EduService.API.Models;
using EduService.Application.Services;
using EduService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel.Models;

namespace EduService.API.Controllers
{
    [Route("api/edu/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly IEduClassService _classService;
        private readonly IEduStudentService _studentService;
        private readonly IMapper _mapper;

        public ClassesController(IEduStudentService studentService, IEduClassService classService, IMapper mapper)
        {
            _studentService = studentService;
            _classService = classService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var classes = await _classService.GetAll();
            var result = _mapper.Map<IEnumerable<EduClassDto>>(classes);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var eduClass = await _classService.GetById(id);
            if (eduClass == null)
                return NotFound();

            var dto = _mapper.Map<EduClassDto>(eduClass);
            return Ok(dto);
        }

        [HttpGet("{id}/students")]
        [AllowAnonymous]
        public IActionResult GetStudents(Guid id)
        {
            var students = _studentService.GetByClassId(id);
            if (students == null)
                return NotFound();

            var result = _mapper.Map<IEnumerable<EduStudentDto>>(students);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduClassDto dto)
        {
            if (dto == null)
                return BadRequest("Class data is required");

            var entity = _mapper.Map<EduClass>(dto);
            var result = await _classService.Create(entity);
            if (result)
                return Ok("Class created successfully");

            return StatusCode(500, "Failed to create class");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduClassDto dto)
        {
            if (dto == null || id != dto.ClassID.ToString())
                return BadRequest("Invalid class data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _classService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduClass>(dto);
            var result = await _classService.Update(entity);
            if (result)
                return Ok("Class updated successfully");

            return StatusCode(500, "Failed to update class");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _classService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _classService.Delete(guidId);
            if (result)
                return Ok("Class deleted successfully");

            return StatusCode(500, "Failed to delete class");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var classes = _classService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduClassDto>>(classes);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
