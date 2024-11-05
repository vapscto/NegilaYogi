using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Exam.LessonPlanner;
using DomainModel.Model.com.vapstech.College.Exam.StudentMentorMapping;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Exam
{
    public class StudentMentorMappingContext: DbContext
    {
        public StudentMentorMappingContext(DbContextOptions<StudentMentorMappingContext> options) : base(options)
        { }
        public DbSet<Clg_Adm_Dept_Course_Branch_MappingDMO> Clg_Adm_Dept_Course_Branch_MappingDMO { get; set; }
        public DbSet<Clg_Adm_Dept_Course_Branch_Semester_MappingDMO> Clg_Adm_Dept_Course_Branch_Semester_MappingDMO { get; set; }
        public DbSet<HR_Master_Department_DMO> HR_Master_Department_DMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<Adm_Course_Branch_MappingDMO> Adm_Course_Branch_MappingDMO { get; set; }
        public DbSet<AdmCourseBranchSemesterMappingDMO> AdmCourseBranchSemesterMappingDMO { get; set; }
        public DbSet<Colleg_Student_Mentor_DetailsDMO> Colleg_Student_Mentor_DetailsDMO { get; set; }
        public DbSet<Clg_Student_Mentor_UserDMO> Clg_Student_Mentor_UserDMO { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get;set; }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Clg_Adm_Dept_Course_Branch_MappingDMO>().HasKey(m => m.ADCO_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Clg_Adm_Dept_Course_Branch_MappingDMO>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
