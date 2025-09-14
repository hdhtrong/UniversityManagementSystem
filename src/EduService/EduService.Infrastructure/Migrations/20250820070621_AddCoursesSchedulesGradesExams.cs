using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoursesSchedulesGradesExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PercentageOfExam",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PercentageOfHomework",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PercentageOfProgress",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseSections",
                columns: table => new
                {
                    SectionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SemesterID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstructorID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaxStudents = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Group = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSections", x => x.SectionID);
                    table.ForeignKey(
                        name: "FK_CourseSections_Instructors_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Instructors",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CourseSections_Semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "Semesters",
                        principalColumn: "SemesterID");
                    table.ForeignKey(
                        name: "FK_CourseSections_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID");
                });

            migrationBuilder.CreateTable(
                name: "ExamSessions",
                columns: table => new
                {
                    ExamSessionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SemesterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSessions", x => x.ExamSessionID);
                    table.ForeignKey(
                        name: "FK_ExamSessions_Semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "Semesters",
                        principalColumn: "SemesterID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SectionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_Enrollments_CourseSections_SectionID",
                        column: x => x.SectionID,
                        principalTable: "CourseSections",
                        principalColumn: "SectionID");
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartPeriod = table.Column<int>(type: "int", nullable: false),
                    EndPeriod = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoomID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK_Schedules_CourseSections_SectionID",
                        column: x => x.SectionID,
                        principalTable: "CourseSections",
                        principalColumn: "SectionID");
                    table.ForeignKey(
                        name: "FK_Schedules_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    ExamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Attendees = table.Column<int>(type: "int", nullable: true),
                    ExamSessionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartPeriod = table.Column<int>(type: "int", nullable: true),
                    EndPeriod = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RoomID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.ExamID);
                    table.ForeignKey(
                        name: "FK_Exams_CourseSections_SectionID",
                        column: x => x.SectionID,
                        principalTable: "CourseSections",
                        principalColumn: "SectionID");
                    table.ForeignKey(
                        name: "FK_Exams_ExamSessions_ExamSessionID",
                        column: x => x.ExamSessionID,
                        principalTable: "ExamSessions",
                        principalColumn: "ExamSessionID");
                    table.ForeignKey(
                        name: "FK_Exams_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignmentScore = table.Column<double>(type: "float", nullable: true),
                    MidtermScore = table.Column<double>(type: "float", nullable: true),
                    FinalExamScore = table.Column<double>(type: "float", nullable: true),
                    Total100Score = table.Column<double>(type: "float", nullable: true),
                    Total10Score = table.Column<double>(type: "float", nullable: true),
                    Total4Score = table.Column<double>(type: "float", nullable: true),
                    LetterGrade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Passed = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeID);
                    table.ForeignKey(
                        name: "FK_Grades_Enrollments_EnrollmentID",
                        column: x => x.EnrollmentID,
                        principalTable: "Enrollments",
                        principalColumn: "EnrollmentID");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleWeeks",
                columns: table => new
                {
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleWeeks", x => new { x.ScheduleID, x.WeekID });
                    table.ForeignKey(
                        name: "FK_ScheduleWeeks_Schedules_ScheduleID",
                        column: x => x.ScheduleID,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleWeeks_Weeks_WeekID",
                        column: x => x.WeekID,
                        principalTable: "Weeks",
                        principalColumn: "WeekID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendances_Enrollments_EnrollmentID",
                        column: x => x.EnrollmentID,
                        principalTable: "Enrollments",
                        principalColumn: "EnrollmentID");
                    table.ForeignKey(
                        name: "FK_Attendances_ScheduleWeeks_ScheduleID_WeekID",
                        columns: x => new { x.ScheduleID, x.WeekID },
                        principalTable: "ScheduleWeeks",
                        principalColumns: new[] { "ScheduleID", "WeekID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EnrollmentID",
                table: "Attendances",
                column: "EnrollmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ScheduleID_WeekID",
                table: "Attendances",
                columns: new[] { "ScheduleID", "WeekID" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSections_Code",
                table: "CourseSections",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSections_InstructorID",
                table: "CourseSections",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSections_SemesterID",
                table: "CourseSections",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSections_SubjectID",
                table: "CourseSections",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_Code",
                table: "Enrollments",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SectionID",
                table: "Enrollments",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentID",
                table: "Enrollments",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_Code",
                table: "Exams",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExamSessionID",
                table: "Exams",
                column: "ExamSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_RoomID",
                table: "Exams",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SectionID",
                table: "Exams",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_Code",
                table: "ExamSessions",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_SemesterID",
                table: "ExamSessions",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_EnrollmentID",
                table: "Grades",
                column: "EnrollmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_RoomID",
                table: "Schedules",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SectionID",
                table: "Schedules",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleWeeks_WeekID",
                table: "ScheduleWeeks",
                column: "WeekID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "ScheduleWeeks");

            migrationBuilder.DropTable(
                name: "ExamSessions");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "CourseSections");

            migrationBuilder.DropColumn(
                name: "PercentageOfExam",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "PercentageOfHomework",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "PercentageOfProgress",
                table: "Subjects");
        }
    }
}
