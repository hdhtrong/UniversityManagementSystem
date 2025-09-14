using Asp.Versioning;
using AuthService.API.Models;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(dtos);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var entity = await _service.GetByNameAsync(name);
            if (entity == null) return NotFound();
            var dto = _mapper.Map<CredentialDto>(entity);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CredentialDto dto)
        {
            if(dto == null || string.IsNullOrEmpty(dto.Name)) return BadRequest();
            var check = await _service.GetByNameAsync(dto.Name);
            if(check != null) return BadRequest(dto.Name + " already exists!");
            var entity = _mapper.Map<AppCredential>(dto);
            await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetByName), new { name = dto.Name }, dto);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Update(string name, [FromBody] CredentialDto dto)
        {
            var entity = _mapper.Map<AppCredential>(dto);
            await _service.UpdateAsync(name, entity);
            return NoContent();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await _service.DeleteAsync(name);
            return NoContent();
        }
    }
}
