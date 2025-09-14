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
    public class InstructorsController : ControllerBase
    {
        private readonly IEduInstructorService _instructorService;
        private readonly IEduDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public InstructorsController(IEduInstructorService instructorService, IEduDepartmentService departmentService, IMapper mapper)
        {
            _instructorService = instructorService;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var instructors = await _instructorService.GetAll();
            var result = _mapper.Map<IEnumerable<EduInstructorDto>>(instructors);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var instructor = await _instructorService.GetById(id);
            if (instructor == null)
                return NotFound();

            var dto = _mapper.Map<EduInstructorDto>(instructor);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduInstructorDto dto)
        {
            if (dto == null)
                return BadRequest("Instructor data is required");

            if(dto.DepartmentID.HasValue)
            {
                var checkDept = _departmentService.GetById(dto.DepartmentID.Value);
                if(checkDept == null)
                    return BadRequest("Department does not exist");
            }

            var entity = _mapper.Map<EduInstructor>(dto);
            var result = await _instructorService.Create(entity);
            if (result)
                return Ok("Instructor created successfully");

            return StatusCode(500, "Failed to create instructor");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduInstructorDto dto)
        {
            if (dto == null || id != dto.ID.ToString())
                return BadRequest("Invalid instructor data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existingInstructor = await _instructorService.GetById(guidId);
            if (existingInstructor == null)
                return NotFound();

            var entity = _mapper.Map<EduInstructor>(dto);
            var result = await _instructorService.Update(entity);
            if (result)
                return Ok("Instructor updated successfully");

            return StatusCode(500, "Failed to update instructor");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existingInstructor = await _instructorService.GetById(guidId);
            if (existingInstructor == null)
                return NotFound();

            var result = await _instructorService.Delete(guidId);
            if (result)
                return Ok("Instructor deleted successfully");

            return StatusCode(500, "Failed to delete instructor");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var instructors = _instructorService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduInstructorDto>>(instructors);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
