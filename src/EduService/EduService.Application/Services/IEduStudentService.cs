using EduService.Application.Dtos;
using EduService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduStudentService
    {
        Task<EduStudentDetailDto?> GetStudentDetailAsync(Guid studentId);

        Task<EduStudent> GetById(Guid id);

        Task<EduStudent> GetByStudentId(string studentId);

        Task<IEnumerable<EduStudent>> GetAll();

        IEnumerable<EduStudent> GetByClassId(Guid classId);

        IQueryable<EduStudent> GetByFilterPaging(FilterRequest filter, out int total);

        Task<ImportResultDto> ImportStudentsAsync(IFormFile file);

        Task<bool> Create(EduStudent st);

        Task<bool> Update(EduStudent st);

        Task<bool> Delete(Guid id);

    }
}
