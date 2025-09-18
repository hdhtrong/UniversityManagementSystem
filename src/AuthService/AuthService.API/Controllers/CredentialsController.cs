using Asp.Versioning;
using AuthService.API.Models;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Models;

namespace AuthService.API.Controllers
{
    [Route("api/auth/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class CredentialsController : ControllerBase
    {
        private readonly ICredentialService _service;
        private readonly IMapper _mapper;

        public CredentialsController(ICredentialService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _service.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CredentialDto>>(entities);
            return Ok(new ApiResponse("Fetched all credentials successfully", dtos));
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var entity = await _service.GetByNameAsync(name);
            if (entity == null)
                return NotFound(new ApiResponse($"Credential '{name}' not found"));

            var dto = _mapper.Map<CredentialDto>(entity);
            return Ok(new ApiResponse("Fetched credential successfully", dto));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CredentialDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new ApiResponse("Invalid credential data"));

            var existing = await _service.GetByNameAsync(dto.Name);
            if (existing != null)
                return BadRequest(new ApiResponse($"Credential '{dto.Name}' already exists"));

            var entity = _mapper.Map<AppCredential>(dto);
            await _service.CreateAsync(entity);

            return Ok(new ApiResponse("Credential created successfully", dto));
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Update(string name, [FromBody] CredentialDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Invalid credential data"));

            var entity = _mapper.Map<AppCredential>(dto);
            await _service.UpdateAsync(name, entity);

            return Ok(new ApiResponse($"Credential '{name}' updated successfully", dto));
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await _service.DeleteAsync(name);
            return Ok(new ApiResponse($"Credential '{name}' deleted successfully"));
        }
    }
}
