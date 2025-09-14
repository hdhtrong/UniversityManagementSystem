using EduService.Domain.Entities;
using EduService.Infrastructure;
using EduService.Application.Extentions;
using Shared.SharedKernel.Models;
using EduService.Application.Dtos;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using ClosedXML.Excel;

namespace EduService.Application.Services.Implementations
{
    public class EduStudentService : IEduStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EduStudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ImportResultDto> ImportStudentsAsync(IFormFile file)
        {
            var result = new ImportResultDto();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

            var studentDtos = new List<EduStudentImportDto>();
            int rowIndex = 2;

            foreach (var row in rows)
            {
                try
                {
                    var dto = ParseRow(row);

                    // business validation
                    ValidateDto(dto);

                    // check tồn tại
                    if (await _unitOfWork.StudentRepository.ExistsByStudentIdAsync(dto.StudentID))
                        throw new Exception("MSSV đã tồn tại: " + dto.StudentID);

                    studentDtos.Add(dto);
                    result.ImportSuccessCount++;
                }
                catch (Exception ex)
                {
                    result.ImportErrors.Add(new ImportErrorDto
                    {
                        Row = rowIndex,
                        Message = ex.Message
                    });
                }
                rowIndex++;
            }

            result.ImportTotal = result.ImportSuccessCount + result.ImportErrorCount;

            if (studentDtos.Any())
            {
                var entities = _mapper.Map<List<EduStudent>>(studentDtos);
                result.SavedTotal = await _unitOfWork.StudentRepository.AddRangeAsync(entities);
            }

            return result;
        }

        private EduStudentImportDto ParseRow(IXLRangeRow row)
        {
            return new EduStudentImportDto
            {
                LastName = row.Cell(2).GetString(),
                FirstName = row.Cell(3).GetString(),
                DOB = row.Cell(4).GetString(),
                Gender = row.Cell(5).GetString(),
                Email = row.Cell(6).GetString(),
                Phone = row.Cell(7).GetString(),
                IdentityNumber = row.Cell(8).GetString(),
                TrainingInstitution = row.Cell(9).GetString(),
                SocialInsuranceNumber = row.Cell(10).GetString(),
                Nationality = row.Cell(11).GetString(),
                Ethnicity = row.Cell(12).GetString(),
                Religion = row.Cell(13).GetString(),
                DisabilityType = row.Cell(14).GetString(),
                ProvincePermanentAddress = row.Cell(15).GetString(),
                DistrictPermanentAddress = row.Cell(16).GetString(),
                WardPermanentAddress = row.Cell(17).GetString(),
                HouseNumberPermanentAddress = row.Cell(18).GetString(),
                ProvincePlaceOfBirth = row.Cell(19).GetString(),
                DistrictPlaceOfBirth = row.Cell(20).GetString(),
                WardPlaceOfBirth = row.Cell(21).GetString(),
                ProvinceHometown = row.Cell(22).GetString(),
                DistrictHometown = row.Cell(23).GetString(),
                WardHometown = row.Cell(24).GetString(),
                JoinYouthUnionDate = row.Cell(25).GetString(),
                JoinPartyDate = row.Cell(26).GetString(),
                OfficialPartyDate = row.Cell(27).GetString(),
                StudentID = row.Cell(28).GetString(),
                AdmissionCategory = row.Cell(29).GetString(),
                AdmissionDecisionNumber = row.Cell(30).GetString(),
                AdmissionDecisionDate = row.Cell(31).GetString(),
                AdmissionResult = row.Cell(32).GetString(),
                ProgramCode = row.Cell(33).GetString(),
                MajorCode = row.Cell(34).GetString(),
                EducationType = row.Cell(35).GetString(),
                TrainingFromYear = row.Cell(36).GetNullable<int>(),
                TrainingToYear = row.Cell(37).GetNullable<int>(),
                DepartmentName = row.Cell(38).GetString(),
                ClassName = row.Cell(39).GetString(),
                IsLinkedDegree = row.Cell(40).GetNullable<bool>(),
                IsDormitoryResident = row.Cell(41).GetNullable<bool>(),
                TemporaryAddress = row.Cell(42).GetString(),
                EnrollmentDate = row.Cell(43).GetString(),
                StudyStatus = row.Cell(44).GetString(),
                StatusChangeDate = row.Cell(45).GetString(),
                StatusDecisionNumber = row.Cell(46).GetString(),
                GraduationDate = row.Cell(47).GetString(),
                GraduationType = row.Cell(48).GetString(),
                GraduationDecisionNumber = row.Cell(49).GetString(),
                GraduationDecisionDate = row.Cell(50).GetString(),
            };
        }

        private void ValidateDto(EduStudentImportDto dto)
        {
            if (string.IsNullOrEmpty(dto.IdentityNumber))
                throw new Exception("Số căn cước/hộ chiếu không được để trống");
            if (string.IsNullOrEmpty(dto.StudentID))
                throw new Exception("Mã sinh viên không được để trống");
            if (string.IsNullOrEmpty(dto.LastName))
                throw new Exception("Họ và tên lót không được để trống");
            if (string.IsNullOrEmpty(dto.FirstName))
                throw new Exception("Tên không được để trống");
            if (string.IsNullOrEmpty(dto.DOB))
                throw new Exception("Ngày sinh không hợp lệ");
        }

        public async Task<bool> Create(EduStudent st)
        {
            if (st != null)
            {
                await _unitOfWork.StudentRepository.Add(st);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var s = await _unitOfWork.StudentRepository.GetById(id);
            if (s != null)
            {
                _unitOfWork.StudentRepository.Delete(s);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduStudent>> GetAll()
        {
            return await _unitOfWork.StudentRepository.GetAll();
        }

        public IEnumerable<EduStudent> GetByClassId(Guid classId)
        {
            return _unitOfWork.StudentRepository.GetMultiByConditions(st => st.ClassID.HasValue && st.ClassID.Value == classId)
                                                .OrderBy(st => st.StudentID);
        }

        public IQueryable<EduStudent> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "Fullname", "Gender", "DOB", "Email", "StudentID", "IdentityNumber", "Nationality", "TrainingFromYear", "MajorCode", "StudyStatus", "EducationType" };
            return _unitOfWork.StudentRepository.GetByFilter(filter, out total, allowedFields, null);
        }

        public async Task<EduStudent> GetById(Guid id)
        {
            return await _unitOfWork.StudentRepository.GetById(id);
        }

        public async Task<EduStudent> GetByStudentId(string studentId)
        {
            return await _unitOfWork.StudentRepository.GetSingleByConditions(s => s.StudentID.Equals(studentId));
        }

        public async Task<EduStudentDetailDto?> GetStudentDetailAsync(Guid studentId)
        {
            var s = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (s == null) return null;

            return new EduStudentDetailDto
            {
                Fullname = s.Fullname,
                DOB = s.DOB,
                Gender = s.Gender,
                Email = s.Email,
                Phone = s.Phone,
                StudentID = s.StudentID,
                IdentityNumber = s.IdentityNumber,
                Nationality = s.Nationality,
                Ethnicity = s.Ethnicity,
                Religion = s.Religion,
                ProvincePermanentAddress = s.ProvincePermanentAddress,   // Tỉnh/thành phố (thường trú)
                DistrictPermanentAddress = s.DistrictPermanentAddress,   // Quận/Huyện (thường trú)
                WardPermanentAddress = s.WardPermanentAddress,           // Xã/Phường (thường trú)
                HouseNumberPermanentAddress = s.HouseNumberPermanentAddress, // Số nhà (thường trú)
                TemporaryAddress = s.TemporaryAddress,   // Địa chỉ tạm trú
                EducationType = s.EducationType,         // Loại hình đào tạo

                MajorName = s.Class?.Program?.Major?.MajorName,
                MajorCode = s.Class?.Program?.Major?.MajorCode,
                ClassCode = s.Class?.ClassCode,
                ClassName = s.Class?.ClassName,
                ProgramName = s.Class?.Program?.ProgramName,
                ProgramCode = s.Class?.Program?.ProgramCode,
                TrainingFromYear = s.TrainingFromYear,   // Đào tạo từ năm
                DepartmentName = s.Class?.Program?.Major?.Department?.DepartmentName,

                InstructorName = s.Class?.Instructor?.Fullname,
                InstructorEmail = s.Class?.Instructor?.Email
            };
        }

        public async Task<bool> Update(EduStudent s)
        {
            if (s != null)
            {
                _unitOfWork.StudentRepository.Update(s);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
