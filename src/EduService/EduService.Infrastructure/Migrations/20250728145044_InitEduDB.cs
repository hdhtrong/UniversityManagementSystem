using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitEduDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOB = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudentID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BankAccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MajorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MajorCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClassCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mentor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentID",
                table: "Students",
                column: "StudentID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
