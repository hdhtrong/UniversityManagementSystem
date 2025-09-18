using Asp.Versioning;
using AutoMapper;
using EduService.API.Models;
using EduService.Application.Services;
using EduService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel.Models;
using SharedKernel.Models;

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
            return Ok(new ApiResponse("Fetched all classes", result));
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var eduClass = await _classService.GetById(id);
            if (eduClass == null)
                return NotFound(new ApiResponse("Class not found"));

            var dto = _mapper.Map<EduClassDto>(eduClass);
            return Ok(new ApiResponse("Class fetched successfully", dto));
        }

        [HttpGet("{id:guid}/students")]
        [AllowAnonymous]
        public IActionResult GetStudents(Guid id)
        {
            var students = _studentService.GetByClassId(id);
            if (students == null)
                return NotFound(new ApiResponse("No students found for this class"));

            var result = _mapper.Map<IEnumerable<EduStudentDto>>(students);
            return Ok(new ApiResponse("Fetched students by class", result));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduClassDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Class data is required"));

            var entity = _mapper.Map<EduClass>(dto);
            var result = await _classService.Create(entity);

            if (!result)
                return StatusCode(500, new ApiResponse("Failed to create class"));

            return CreatedAtAction(nameof(GetById), new { id = entity.ClassID },
                new ApiResponse("Class created successfully", dto));
        }

        [HttpPut("{id:guid}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EduClassDto dto)
        {
            if (dto == null || id != dto.ClassID)
                return BadRequest(new ApiResponse("Invalid class data"));

            var existing = await _classService.GetById(id);
            if (existing == null)
                return NotFound(new ApiResponse("Class not found"));

            var entity = _mapper.Map<EduClass>(dto);
            var result = await _classService.Update(entity);

            if (!result)
                return StatusCode(500, new ApiResponse("Failed to update class"));

            return Ok(new ApiResponse("Class updated successfully", dto));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _classService.GetById(id);
            if (existing == null)
                return NotFound(new ApiResponse("Class not found"));

            var result = await _classService.Delete(id);
            if (!result)
                return StatusCode(500, new ApiResponse("Failed to delete class"));

            return Ok(new ApiResponse("Class deleted successfully"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var classes = _classService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduClassDto>>(classes);

            var response = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Paged classes fetched successfully", response));
        }
    }
}
