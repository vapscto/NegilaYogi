using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Exam.StudentMentor;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.Exam
{
    public class StudentMentorContext : DbContext
    {
        public StudentMentorContext(DbContextOptions<StudentMentorContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000000);
        }
        public StudentMentorContext()
        {
        }
        public DbSet<School_Adm_Master_MentorDMO> School_Adm_Master_MentorDMO { get; set; }
        public DbSet<School_Adm_Master_Mentor_MenteeDMO> School_Adm_Master_Mentor_MenteeDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<Exm_Login_PrivilegeDMO> Exm_Login_PrivilegeDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_SubjectsDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubSubjectsDMO> Exm_Login_Privilege_SubSubjectsDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }     
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<School_Adm_Master_MentorDMO>().ToTable("Adm_Master_Mentor");
            
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();          

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
