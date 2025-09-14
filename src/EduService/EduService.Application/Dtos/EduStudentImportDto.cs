namespace EduService.Application.Dtos
{
    public class EduStudentImportDto
    {
        public string? LastName { get; set; } 
        public string? FirstName { get; set; } 
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? TrainingInstitution { get; set; }
        public string? SocialInsuranceNumber { get; set; }
        public string? Nationality { get; set; }
        public string? Ethnicity { get; set; }
        public string? Religion { get; set; }
        public string? DisabilityType { get; set; }
        public string? ProvincePermanentAddress { get; set; } // Tỉnh/thành phố (thường trú)
        public string? DistrictPermanentAddress { get; set; } // Quận/Huyện (thường trú)
        public string? WardPermanentAddress { get; set; } // Xã/Phường (thường trú)
        public string? HouseNumberPermanentAddress { get; set; } // Số nhà (thường trú)
        public string? ProvincePlaceOfBirth { get; set; } // Tỉnh/thành phố (Nơi sinh)
        public string? DistrictPlaceOfBirth { get; set; } // Quận/Huyện (Nơi sinh)
        public string? WardPlaceOfBirth { get; set; } // Xã/Phường (Nơi sinh)
        public string? ProvinceHometown { get; set; } // Tỉnh/thành phố (Quê quán)
        public string? DistrictHometown { get; set; } // Quận/Huyện (Quê quán)
        public string? WardHometown { get; set; } // Xã/Phường (Quê quán)
        public string? JoinYouthUnionDate { get; set; }
        public string? JoinPartyDate { get; set; }
        public string? OfficialPartyDate { get; set; }
        public required string StudentID { get; set; }
        public string? AdmissionCategory { get; set; }
        public string? AdmissionDecisionNumber { get; set; }
        public string? AdmissionDecisionDate { get; set; }
        public string? AdmissionResult { get; set; }
        public string? ProgramCode { get; set; }
        public string? MajorCode { get; set; }
        public string? EducationType { get; set; }
        public int? TrainingFromYear { get; set; }
        public int? TrainingToYear { get; set; }
        public string? DepartmentName { get; set; }
        public string? ClassName { get; set; }
        public bool? IsLinkedDegree { get; set; }
        public bool? IsDormitoryResident { get; set; }
        public string? TemporaryAddress { get; set; }
        public string? EnrollmentDate { get; set; }
        public string? StudyStatus { get; set; }
        public string? StatusChangeDate { get; set; }
        public string? StatusDecisionNumber { get; set; }
        public string? GraduationDate { get; set; }
        public string? GraduationType { get; set; }
        public string? GraduationDecisionNumber { get; set; }
        public string? GraduationDecisionDate { get; set; }
    }

}
