using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Exam;
using DomainModel.Model.SeatingArrangment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.SeatingArrangment
{
    public class SAMasterBuildingContext : DbContext
    {
        public SAMasterBuildingContext(DbContextOptions<SAMasterBuildingContext> options) : base(options)
        { }
        public DbSet<AdmCollegeSubjectSchemeDMO> AdmCollegeSubjectSchemeDMO { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_SubjectsDMO { get; set; }
        public DbSet<Exam_SA_ETT_DetailsDMO> Exam_SA_ETT_DetailsDMO { get; set; }
        public DbSet<Exam_SA_ETTDMO> Exam_SA_ETTDMO { get; set; }
        public DbSet<Exam_SA_ChiefCoordinatorDMO> Exam_SA_ChiefCoordinatorDMO { get; set; }
        public DbSet<Exam_SA_Malpractice_StudentDMO> Exam_SA_Malpractice_StudentDMO { get; set; }
        public DbSet<Exam_SA_Absent_StudentDMO> Exam_SA_Absent_StudentDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<exammasterDMO> exammasterDMO { get; set; }
        public DbSet<Exam_SA_SuperintendentDMO> Exam_SA_SuperintendentDMO { get; set; }
        public DbSet<AcademicYear> AcademicYearDMO { get; set; }
        public DbSet<Exam_SA_BuildingDMO> Exam_SA_BuildingDMO { get; set; }
        public DbSet<Exam_SA_FloorDMO> Exam_SA_FloorDMO { get; set; }
        public DbSet<Exam_SA_RoomDMO> Exam_SA_RoomDMO { get; set; }
        public DbSet<Exam_SA_University_ExamDMO> Exam_SA_University_ExamDMO { get; set; }
        public DbSet<Exam_SA_Allot_Staff_DutyTypeDMO> Exam_SA_Allot_Staff_DutyTypeDMO { get; set; }
        public DbSet<Exam_SA_ExamSlotDMO> Exam_SA_ExamSlotDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<Exm_Col_Yearly_SchemeDMO> Exm_Col_Yearly_SchemeDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_GroupDMO> Exm_Col_Yearly_Scheme_GroupDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_Group_SubjectsDMO> Exm_Col_Yearly_Scheme_Group_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<Exm_Col_Studentwise_SubjectsDMO> Exm_Col_Studentwise_SubjectsDMO { get; set; }
        public DbSet<Exam_SA_ExamDateDMO> Exam_SA_ExamDateDMO { get; set; }
        public DbSet<Exam_SA_ExamDate_RoomDMO> Exam_SA_ExamDate_RoomDMO { get; set; }
        public DbSet<Exam_SA_Room_Sitting_StyleDMO> Exam_SA_Room_Sitting_StyleDMO { get; set; }
        public DbSet<School_Exam_SA_ExamDate_SchoolDMO> School_Exam_SA_ExamDate_SchoolDMO { get; set; }
        public DbSet<School_Exam_SA_ExamDate_Room_SchoolDMO> School_Exam_SA_ExamDate_Room_SchoolDMO { get; set; }
        public DbSet<School_Exam_SA_Room_SchoolDMO> School_Exam_SA_Room_SchoolDMO { get; set; }
        public DbSet<School_Exam_SA_Room_ClassSubject_SchoolDMO> School_Exam_SA_Room_ClassSubject_SchoolDMO { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<School_Exam_SA_Absent_Student_SchoolDMO> School_Exam_SA_Absent_Student_SchoolDMO { get; set; }
        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }
        public DbSet<Exm_Yearly_CategoryDMO> Exm_Yearly_CategoryDMO { get; set; }
        public DbSet<Exm_Yearly_Category_ExamsDMO> Exm_Yearly_Category_ExamsDMO { get; set; }
        public DbSet<Exm_Yearly_Category_GroupDMO> Exm_Yearly_Category_GroupDMO { get; set; }
        public DbSet<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_SubjectsDMO { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_Student { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Exam_SA_BuildingDMO>().HasKey(m => m.ESABLD_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();          

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {               
            }
        }
    }
}