using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ORDService.API.Models;

namespace ORDService.API.Controllers
{
    [Route("api/ord/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    public class ResearchProjectsController : ControllerBase
    {
        // Dữ liệu giả (fake data)
        private static readonly List<ResearchProjectDto> _mockProjects = new List<ResearchProjectDto>
        {
            new ResearchProjectDto { Id = 1, Title = "AI trong giáo dục", Description = "Nghiên cứu ứng dụng AI trong hệ thống học tập", StartDate = "2023-01-01", EndDate = "2023-12-31" },
            new ResearchProjectDto { Id = 2, Title = "Phân tích dữ liệu lớn", Description = "Khai phá dữ liệu lớn trong thương mại điện tử", StartDate = "2022-06-01", EndDate = "2023-06-30" },
        };

        // GET: api/ord/v1/ResearchProjects
        [HttpGet]
        public ActionResult<IEnumerable<ResearchProjectDto>> GetAll()
        {
            return Ok(_mockProjects);
        }

        // GET: api/ord/v1/ResearchProjects/1
        [HttpGet("{id}")]
        public ActionResult<ResearchProjectDto> GetById(int id)
        {
            var project = _mockProjects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound($"Không tìm thấy dự án với Id = {id}");

            return Ok(project);
        }
    }
}
