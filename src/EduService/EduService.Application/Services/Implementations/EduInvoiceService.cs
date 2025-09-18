using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduInvoiceService : IEduInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduInvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduInvoice entity)
        {
            if (entity != null)
            {
                await _unitOfWork.InvoiceRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.InvoiceRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.InvoiceRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduInvoice>> GetAll()
        {
            return await _unitOfWork.InvoiceRepository.GetAll();
        }

        public IQueryable<EduInvoice> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "InvoiceNumber", "StudentID", "SemesterCode", "TuitionFeeID" };
            return _unitOfWork.InvoiceRepository.GetByFilter(filter, out total, allowedFields, ["TuitionFee", "TuitionFee.Student", "TuitionFee.Semester"]);
        }

        public async Task<EduInvoice> GetById(Guid id)
        {
            return await _unitOfWork.InvoiceRepository.GetById(id);
        }

        public async Task<bool> Update(EduInvoice entity)
        {
            if (entity != null)
            {
                _unitOfWork.InvoiceRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
