using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class ScheduleReportContext : DbContext
    {
        public ScheduleReportContext(DbContextOptions<ScheduleReportContext> options) :base(options)
        { }
        public ScheduleReportContext()
        {
        }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<School_M_Class> admissioncls { get; set; }

        public DbSet<School_M_Section> school_M_Section { get; set; }

        public DbSet<Adm_M_Student> AdmissionStudentDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }

        public DbSet<Preadmission_SeatBlocked_Student> seat_blocked { get; set; }
        public DbSet<StudentApplication> student_registration { get; set; }
        public DbSet<WrittenTestScheduleDMO> writentest { get; set; }
        public DbSet<OralTestScheduleDMO> oraltest { get; set; }

        public DbSet<StudentApplication> StudentApplication { get; set; }

        public DbSet<StudentSibling> StudentSibling { get; set; }

        public DbSet<Institution> institution { get; set; }
        public DbSet<OralTestScheduleStudentInsertDMO> oralstudents { get; set; }

        //added praveen
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }

    }
}
