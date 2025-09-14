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
    public class TuitionFeesController : ControllerBase
    {
        private readonly IEduTuitionFeeService _tuitionFeeService;
        private readonly IMapper _mapper;

        public TuitionFeesController(IEduTuitionFeeService tuitionFeeService, IMapper mapper)
        {
            _tuitionFeeService = tuitionFeeService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var fees = await _tuitionFeeService.GetAll();
            var result = _mapper.Map<IEnumerable<EduTuitionFeeDto>>(fees);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var fee = await _tuitionFeeService.GetById(id);
            if (fee == null)
                return NotFound();

            var dto = _mapper.Map<EduTuitionFeeDto>(fee);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduTuitionFeeDto dto)
        {
            if (dto == null)
                return BadRequest("Tuition fee data is required");

            var entity = _mapper.Map<EduTuitionFee>(dto);
            var result = await _tuitionFeeService.Create(entity);

            if (result)
                return Ok("Tuition fee created successfully");

            return StatusCode(500, "Failed to create tuition fee");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduTuitionFeeDto dto)
        {
            if (dto == null || id != dto.TuitionFeeID.ToString())
                return BadRequest("Invalid tuition fee data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _tuitionFeeService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduTuitionFee>(dto);
            var result = await _tuitionFeeService.Update(entity);

            if (result)
                return Ok("Tuition fee updated successfully");

            return StatusCode(500, "Failed to update tuition fee");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _tuitionFeeService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _tuitionFeeService.Delete(guidId);
            if (result)
                return Ok("Tuition fee deleted successfully");

            return StatusCode(500, "Failed to delete tuition fee");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var fees = _tuitionFeeService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduTuitionFeeDto>>(fees);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
