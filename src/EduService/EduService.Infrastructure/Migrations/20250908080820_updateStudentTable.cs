using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MajorName",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Students",
                newName: "StatusDecisionNumber");

            migrationBuilder.RenameColumn(
                name: "BankAccountNumber",
                table: "Students",
                newName: "StatusChangeDate");

            migrationBuilder.RenameColumn(
                name: "BankAccountName",
                table: "Students",
                newName: "SocialInsuranceNumber");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Students",
                newName: "TemporaryAddress");

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MajorCode",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "AdmissionCategory",
                table: "Students",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdmissionDecisionDate",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdmissionDecisionNumber",
                table: "Students",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdmissionResult",
                table: "Students",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "Students",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Students",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisabilityType",
                table: "Students",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictHometown",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictPermanentAddress",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictPlaceOfBirth",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationType",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnrollmentDate",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ethnicity",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraduationDate",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraduationDecisionDate",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraduationDecisionNumber",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraduationType",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumberPermanentAddress",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDormitoryResident",
                table: "Students",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLinkedDegree",
                table: "Students",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoinPartyDate",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoinYouthUnionDate",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialPartyDate",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProgramCode",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceHometown",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvincePermanentAddress",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvincePlaceOfBirth",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudyStatus",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainingFromYear",
                table: "Students",
                type: "int",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingInstitution",
                table: "Students",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainingToYear",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardHometown",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardPermanentAddress",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardPlaceOfBirth",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionCategory",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AdmissionDecisionDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AdmissionDecisionNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AdmissionResult",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DisabilityType",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DistrictHometown",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DistrictPermanentAddress",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DistrictPlaceOfBirth",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EducationType",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Ethnicity",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GraduationDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GraduationDecisionDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GraduationDecisionNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GraduationType",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "HouseNumberPermanentAddress",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsDormitoryResident",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsLinkedDegree",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "JoinPartyDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "JoinYouthUnionDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "OfficialPartyDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProgramCode",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProvinceHometown",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProvincePermanentAddress",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProvincePlaceOfBirth",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudyStatus",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TrainingFromYear",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TrainingInstitution",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TrainingToYear",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "WardHometown",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "WardPermanentAddress",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "WardPlaceOfBirth",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "TemporaryAddress",
                table: "Students",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "StatusDecisionNumber",
                table: "Students",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "StatusChangeDate",
                table: "Students",
                newName: "BankAccountNumber");

            migrationBuilder.RenameColumn(
                name: "SocialInsuranceNumber",
                table: "Students",
                newName: "BankAccountName");

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Students",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MajorCode",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MajorName",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
