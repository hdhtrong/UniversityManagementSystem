using System.ComponentModel.DataAnnotations;

namespace EduService.API.Models
{
    public class EduStudentDto
    {
        public Guid? ID { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; } // Tên

        [MaxLength(100)]
        public string? MiddleName { get; set; } // Họ và tên đệm

        [MaxLength(100)]
        public required string Fullname { get; set; } // Họ và tên đầy đủ

        [MaxLength(50)]
        public required string DOB { get; set; } // Ngày tháng năm sinh

        [MaxLength(50)]
        public string? Gender { get; set; } // Giới tính

        [MaxLength(150)]
        public string? Email { get; set; } // Email

        [MaxLength(100)]
        public string? Phone { get; set; } // Điện thoại

        [MaxLength(50)]
        public required string IdentityNumber { get; set; } // Số CCCD/Hộ chiếu

        [MaxLength(150)]
        public string? TrainingInstitution { get; set; } // Cơ sở đào tạo

        [MaxLength(50)]
        public string? SocialInsuranceNumber { get; set; } // Số sổ bảo hiểm

        [MaxLength(100)]
        public string? Nationality { get; set; } // Quốc tịch

        [MaxLength(50)]
        public string? Ethnicity { get; set; } // Dân tộc

        [MaxLength(100)]
        public string? Religion { get; set; } // Tôn giáo

        [MaxLength(250)]
        public string? DisabilityType { get; set; } // Loại khuyết tật

        [MaxLength(100)]
        public string? ProvincePermanentAddress { get; set; } // Tỉnh/thành phố (thường trú)
        [MaxLength(100)]
        public string? DistrictPermanentAddress { get; set; } // Quận/Huyện (thường trú)
        [MaxLength(100)]
        public string? WardPermanentAddress { get; set; } // Xã/Phường (thường trú)
        [MaxLength(100)]
        public string? HouseNumberPermanentAddress { get; set; } // Số nhà (thường trú)

        [MaxLength(100)]
        public string? ProvincePlaceOfBirth { get; set; } // Tỉnh/thành phố (Nơi sinh)
        [MaxLength(100)]
        public string? DistrictPlaceOfBirth { get; set; } // Quận/Huyện (Nơi sinh)
        [MaxLength(100)]
        public string? WardPlaceOfBirth { get; set; } // Xã/Phường (Nơi sinh)

        [MaxLength(100)]
        public string? ProvinceHometown { get; set; } // Tỉnh/thành phố (Quê quán)
        [MaxLength(100)]
        public string? DistrictHometown { get; set; } // Quận/Huyện (Quê quán)
        [MaxLength(100)]
        public string? WardHometown { get; set; } // Xã/Phường (Quê quán)

        public string? JoinYouthUnionDate { get; set; } // Ngày vào đoàn
        public string? JoinPartyDate { get; set; } // Ngày vào đảng
        public string? OfficialPartyDate { get; set; } // Ngày vào đảng chính thức

        // ====== Tuyển sinh / Đào tạo ======
        [MaxLength(50)]
        public required string StudentID { get; set; } // Mã sinh viên
        [MaxLength(150)]
        public string? AdmissionCategory { get; set; } // Đối tượng đầu vào
        [MaxLength(150)]
        public string? AdmissionDecisionNumber { get; set; } // Số quyết định trúng tuyển
        [MaxLength(50)]
        public string AdmissionDecisionDate { get; set; } // Ngày ký QĐ trúng tuyển
        [MaxLength(150)]
        public string? AdmissionResult { get; set; } // Kết quả tuyển sinh

        [MaxLength(100)]
        public string? ProgramCode { get; set; } // Mã chương trình đào tạo
        [MaxLength(100)]
        public string? MajorCode { get; set; } // Mã ngành đào tạo
        [MaxLength(100)]
        public string? EducationType { get; set; } // Loại hình đào tạo

        [MaxLength(100)]
        public int? TrainingFromYear { get; set; } // Đào tạo từ năm
        public int? TrainingToYear { get; set; } // Đào tạo đến năm

        [MaxLength(250)]
        public string? DepartmentName { get; set; } // Khoa

        [MaxLength(250)]
        public string? ClassName { get; set; } // Lớp sinh hoạt/Lớp niên chế
        public bool? IsLinkedDegree { get; set; } // Bằng tốt nghiệp liên thông
        public bool? IsDormitoryResident { get; set; } // Đang ở nội trú
        [MaxLength(250)]
        public string? TemporaryAddress { get; set; } // Nơi tạm trú (nếu không nội trú)
        [MaxLength(50)]
        public string? EnrollmentDate { get; set; } // Ngày nhập học
        [MaxLength(100)]
        public string? StudyStatus { get; set; } // Tình trạng học
        [MaxLength(50)]
        public string? StatusChangeDate { get; set; } // Ngày chuyển trạng thái
        [MaxLength(50)]
        public string? StatusDecisionNumber { get; set; } // Số QĐ thôi học/bảo lưu...
        [MaxLength(50)]
        public string? GraduationDate { get; set; } // Thời gian tốt nghiệp
        [MaxLength(50)]
        public string? GraduationType { get; set; } // Loại tốt nghiệp: Giỏi, Khá.v.v
        [MaxLength(50)]
        public string? GraduationDecisionNumber { get; set; } // Số quyết định tốt nghiệp
        [MaxLength(50)]
        public string? GraduationDecisionDate { get; set; } // Ngày QĐ công nhận tốt nghiệp

        // ====== Quan hệ ======
        public Guid? ClassID { get; set; }
    }
}
