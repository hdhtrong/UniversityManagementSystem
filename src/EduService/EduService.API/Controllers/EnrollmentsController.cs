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
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEduEnrollmentService _enrollmentService;
        private readonly IMapper _mapper;

        public EnrollmentsController(IEduEnrollmentService enrollmentService, IMapper mapper)
        {
            _enrollmentService = enrollmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAll();
            var result = _mapper.Map<IEnumerable<EduEnrollmentDto>>(enrollments);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var enrollment = await _enrollmentService.GetById(id);
            if (enrollment == null)
                return NotFound();

            var dto = _mapper.Map<EduEnrollmentDto>(enrollment);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduEnrollmentDto dto)
        {
            if (dto == null)
                return BadRequest("Enrollment data is required");

            var entity = _mapper.Map<EduEnrollment>(dto);
            var result = await _enrollmentService.Create(entity);
            if (result)
                return Ok("Enrollment created successfully");

            return StatusCode(500, "Failed to create enrollment");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduEnrollmentDto dto)
        {
            if (dto == null || id != dto.EnrollmentID.ToString())
                return BadRequest("Invalid enrollment data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _enrollmentService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduEnrollment>(dto);
            var result = await _enrollmentService.Update(entity);
            if (result)
                return Ok("Enrollment updated successfully");

            return StatusCode(500, "Failed to update enrollment");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _enrollmentService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _enrollmentService.Delete(guidId);
            if (result)
                return Ok("Enrollment deleted successfully");

            return StatusCode(500, "Failed to delete enrollment");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var enrollments = _enrollmentService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduEnrollmentDto>>(enrollments);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
