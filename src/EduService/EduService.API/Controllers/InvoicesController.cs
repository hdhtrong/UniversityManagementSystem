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
    public class InvoicesController : ControllerBase
    {
        private readonly IEduInvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public InvoicesController(IEduInvoiceService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoiceService.GetAll();
            var result = _mapper.Map<IEnumerable<EduInvoiceDto>>(invoices);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var invoice = await _invoiceService.GetById(id);
            if (invoice == null)
                return NotFound();

            var dto = _mapper.Map<EduInvoiceDto>(invoice);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduInvoiceDto dto)
        {
            if (dto == null)
                return BadRequest("Invoice data is required");

            var entity = _mapper.Map<EduInvoice>(dto);
            var result = await _invoiceService.Create(entity);

            if (result)
                return Ok("Invoice created successfully");

            return StatusCode(500, "Failed to create invoice");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduInvoiceDto dto)
        {
            if (dto == null || id != dto.InvoiceID.ToString())
                return BadRequest("Invalid invoice data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _invoiceService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduInvoice>(dto);
            var result = await _invoiceService.Update(entity);

            if (result)
                return Ok("Invoice updated successfully");

            return StatusCode(500, "Failed to update invoice");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _invoiceService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _invoiceService.Delete(guidId);
            if (result)
                return Ok("Invoice deleted successfully");

            return StatusCode(500, "Failed to delete invoice");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var invoices = _invoiceService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduInvoiceDto>>(invoices);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
