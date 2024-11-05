using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.FeedBack;
using DomainModel.Model.NAAC.FeedBack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.FeedBack
{
    public class FeedBackContext : DbContext
    {
        public FeedBackContext(DbContextOptions<FeedBackContext> options) : base(options)
        { }
        public DbSet<Alumni_User_LoginDMO> Alumni_User_LoginDMO_con { get; set; }
        public DbSet<FeedBackMasterTypeDMO> FeedBackMasterTypeDMO { get; set; }
        public DbSet<Feedback_Master_QuestionsDMO> Feedback_Master_QuestionsDMO { get; set; }
        public DbSet<Feedback_Master_OptionsDMO> Feedback_Master_OptionsDMO { get; set; }
        public DbSet<Feedback_Type_QuestionsDMO> Feedback_Type_QuestionsDMO { get; set; }
        public DbSet<Feedback_Type_OptionsDMO> Feedback_Type_OptionsDMO { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<Feedback_Type_Questions_OptionsDMO> Feedback_Type_Questions_OptionsDMO { get; set; }
        public DbSet<Feedback_College_Student_TransactionDMO> Feedback_College_Student_TransactionDMO { get; set; }
        public DbSet<Feedback_School_Student_TransactionDMO> Feedback_School_Student_TransactionDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }        
        public DbSet<Feedback_Staff_TransactionDMO> Feedback_Staff_TransactionDMO { get; set; }
        public DbSet<Feedback_Alumni_TransactionDMO> Feedback_Alumni_TransactionDMO { get; set; }
        public DbSet<CLGAlumni_User_LoginDMO> CLGAlumni_User_LoginDMO { get; set; }
        public DbSet<CLGAlumni_M_StudentDMO> CLGAlumni_M_StudentDMO  { get; set; }
        public DbSet<Feedback_CLG_Alumni_TransactionDMO> Feedback_College_Alumni_Transaction { get; set; }
        public DbSet<Feedback_College_StudentToStaffDMO> Feedback_College_StudentToStaffDMO { get; set; }
        public DbSet<Feedback_School_StudentToStaffDMO> Feedback_School_StudentToStaffDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<CollegeStudentlogin> CollegeStudentlogin { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<ApplRole> applicationRole { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }        
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<FeedBackMasterTypeDMO>().HasKey(m => m.FMTY_Id); 
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<FeedBackMasterTypeDMO>();       

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
