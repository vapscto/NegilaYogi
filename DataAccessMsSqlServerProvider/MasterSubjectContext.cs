using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterSubjectContext:DbContext
    {
        public MasterSubjectContext(DbContextOptions<MasterSubjectContext> options) :base(options)
        { }
        public DbSet<MasterSubjectDMO> masterSubject { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_SubjectsDMO { get; set; }
        

        //for Dependency
        public DbSet<TT_Restricting_Period_SubjectDMO> TT_Restricting_Period_SubjectDMO { get; set; }
        public DbSet<TT_Restricting_Period_Staff_ClassSectionDMO> TT_Restricting_Period_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Restricting_Day_SubjectDMO> TT_Restricting_Day_SubjectDMO { get; set; }
        public DbSet<TT_Restricting_Day_Staff_ClassSectionDMO> TT_Restricting_Day_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Restricting_Day_PeriodDMO> TT_Restricting_Day_PeriodDMO { get; set; }
        public DbSet<TT_Master_Subject_AbbreviationDMO> TT_Master_Subject_AbbreviationDMO { get; set; }
        public DbSet<TT_LABLIB_DetailsDMO> TT_LABLIB_DetailsDMO { get; set; }
        public DbSet<TT_Fixing_Period_SubjectDMO> TT_Fixing_Period_SubjectDMO { get; set; }
        public DbSet<TT_Fixing_Period_Staff_ClassSectionDMO> TT_Fixing_Period_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Fixing_Day_SubjectDMO> TT_Fixing_Day_SubjectDMO { get; set; }
        public DbSet<TT_Fixing_Day_Staff_ClassSectionDMO> TT_Fixing_Day_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Fixing_Day_PeriodDMO> TT_Fixing_Day_PeriodDMO { get; set; }
        public DbSet<TT_Final_Period_Distribution_DetailedDMO> TT_Final_Period_Distribution_DetailedDMO { get; set; }
        public DbSet<TT_Final_Generation_DetailedDMO> TT_Final_Generation_DetailedDMO { get; set; }
        public DbSet<TT_ConsecutiveDMO> TT_ConsecutiveDMO { get; set; }
        public DbSet<TT_Bifurcation_Details_DMO> TT_Bifurcation_Details_DMO { get; set; }
        public DbSet<WIrttenTestSubjectWiseMarksDMO> WIrttenTestSubjectWiseMarksDMO { get; set; }
        public DbSet<Exm_Yrly_Cat_Exams_SubwiseDMO> Exm_Yrly_Cat_Exams_SubwiseDMO { get; set; }
        public DbSet<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_SubjectsDMO { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<ExmStudentMarksProcessSubjectwiseDMO> ExmStudentMarksProcessSubjectwiseDMO { get; set; }
        public DbSet<ExamMarksDMO> ExamMarksDMO { get; set; }
        public DbSet<Exm_Master_Group_SubjectsDMO> Exm_Master_Group_SubjectsDMO { get; set; }
        public DbSet<Exm_M_Promotion_SubjectsDMO> Exm_M_Promotion_SubjectsDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_SubjectsDMO { get; set; }
        public DbSet<Adm_studentAttendanceSubjects> Adm_studentAttendanceSubjects { get; set; }
        public DbSet<SubjectwisePeriodSettingsDMO> SubjectwisePeriodSettingsDMO { get; set; }
        public DbSet<AdmSchoolAttendanceSubjectBatch> AdmSchoolAttendanceSubjectBatch { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClassSubject> Adm_schAttLoginUserClassSubjects { get; set; }
        public DbSet<WrittenTestStudentSubjectWiseMarksDMO> WrittenTestStudentSubjectWiseMarksDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
           // base.OnModelCreating(modelbuilder);
            // modelbuilder.Entity<MasterSection>().HasKey(m => m.MO_Id);

            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<IVRM_Master_SubjectsDMO>().ToTable("IVRM_Master_Subjects");
            //// modelbuilder.Entity<>().HasKey(m => m.MOE_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterSubjectDMO>();
            updateUpdatedProperty<IVRM_Master_SubjectsDMO>();

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
