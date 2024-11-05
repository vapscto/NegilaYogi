using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Fee;

namespace DataAccessMsSqlServerProvider
{
    public class StudentAttendancecustomReportContext : DbContext
    {
        public StudentAttendancecustomReportContext(DbContextOptions<StudentAttendancecustomReportContext> options) : base(options)
        { }
        public DbSet<Adm_M_Student> AdmissionStudentDMO { get; set; }

        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<MasterAcademic> academicYear { get; set; }
        public DbSet<School_M_Section> masterSection { get; set; }
        public DbSet<School_M_Class> admissionClass { get; set; }
        public DbSet<MasterMonthDMO> masterMonth { get; set; }
        public DbSet<Adm_M_Student> admissionStduent { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> admissionyearstudent { get; set; }
        public DbSet<Adm_studentAttendance> attendancelist { get; set; }
        public DbSet<Adm_studentAttendanceStudents> attendanceStudentlist { get; set; }
        public DbSet<StudentTC> StudentTcList { get; set; }
        public DbSet<Religion> religion { get; set; }
        public DbSet<mastercasteDMO> caste { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<DistrictDMO> DistrictDMO { get; set; }
        public DbSet<MasterCompanyDMO> companyname { get; set;}

        public DbSet<Institution> Institution { get; set; }
        public DbSet<StudentPrevSchoolDMO> StudentPrevSchoolDMO { get; set; }
        public DbSet<MasterCategory> category { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
    }
}
