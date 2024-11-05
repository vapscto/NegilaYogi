using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class AdmissionFormContext : DbContext
    {
        public AdmissionFormContext(DbContextOptions<AdmissionFormContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000);
        }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }

        public DbSet<MasterAcademic> year { get; set; }
        public DbSet<School_M_Section> AdmSection { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<MasterSubjectDMO> allSubject { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }

        public DbSet<DistrictDMO> DistrictDMO { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<MasterReligionDMO> Religion { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<mastercasteDMO> Caste { get; set; }
        public DbSet<ReligionCategory_MappingDMO> ReligionCategory_MappingDMO { get; set; }
        public DbSet<CasteCategory> CasteCategory { get; set; }
        public DbSet<MasterReference> MasterReference { get; set; }
        public DbSet<MasterSource> MasterSource { get; set; }
        public DbSet<MasterActivityDMO> MasterActivityDMO { get; set; }
        public DbSet<MasterDocumentDMO> MasterDocumentDMO { get; set; }
        public DbSet<MasterStudentBondDMO> MasterStudentBondDMO { get; set; }
        public DbSet<StudentPrevSchoolDMO> StudentPrevSchoolDMO { get; set; }
        public DbSet<StudentGuardianDMO> StudentGuardianDMO { get; set; }
        public DbSet<StudentSiblingDMO> StudentSiblingDMO { get; set; }
        public DbSet<StudentDocumentDMO> StudentDocumentDMO { get; set; }
        public DbSet<StudentAchivementDMO> StudentAchivementDMO { get; set; }
        public DbSet<StudentReferenceDMO> StudentReferenceDMO { get; set; }
        public DbSet<StudentSourceDMO> StudentSourceDMO { get; set; }
        public DbSet<StudentActitvityDMO> StudentActitvityDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> SchoolYearWiseStudent { get; set; }
        public DbSet<readmitstudentDMO> readmitstudentDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }    
        public DbSet<Adm_M_Category> Adm_M_Stu_Cat { get; set; }
        public DbSet<StudentUserLoginDMO> studentUserLoginDMO { get; set; }
        public DbSet<StudentUserLogin_Institutionwise> studentUserLogin_Institutionwise { get; set; }
        public DbSet<StudentAppUserLoginDMO> userstudentapp { get; set; }
        public DbSet<StudentTC> Student_TC { get; set; }
        public DbSet<GovernmentBondDMO> GovernmentBond { get; set; }
        public DbSet<MasterBorad> MasterBorad { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }
        public DbSet<Adm_M_Student_FatherMobileNo> Adm_M_Student_FatherMobileNo { get; set; }
        public DbSet<Adm_Master_Father_Email> Adm_Master_Father_Email { get; set; }
        public DbSet<Adm_M_Mother_MobileNo> Adm_M_Mother_MobileNo { get; set; }
        public DbSet<Adm_M_Mother_Emailid> Adm_M_Mother_Emailid { get; set; }
        public DbSet<Adm_M_Student_MobileNo> Adm_M_Student_MobileNo { get; set; }
        public DbSet<Adm_M_Student_Email_Id> Adm_M_Student_Email_Id { get; set; }
        public DbSet<MasterSchoolType> MasterSchoolType { get; set; }
        public DbSet<StudentApplication> Enq { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<Adm_Student_EcsDetailsDMO> Adm_Student_EcsDetailsDMO { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }
        public DbSet<Exm_Yearly_CategoryDMO> Exm_Yearly_CategoryDMO { get; set; }
        public DbSet<Exm_Yearly_Category_ExamsDMO> Exm_Yearly_Category_ExamsDMO { get; set; }
        public DbSet<exammasterDMO> masterexam { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<SMSMasterApprovalDMO> SMSMasterApprovalDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<SMSEmailSetting> SMSEmailSetting { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Adm_School_Master_Stream> Adm_School_Master_Stream { get; set; }
        public DbSet<StudentCompliants_DMO> StudentCompliants_DMO { get; set; }
        public DbSet<Adm_School_Stream_Class> Adm_School_Stream_Class { get; set; }
        public DbSet<Adm_School_Master_CE> Adm_School_Master_CE { get; set; }
        public DbSet<Adm_School_Stream_Class_CE> Adm_School_Stream_Class_CE { get; set; }
        public DbSet<StudycertificateReportDMO> StudycertificateReportDMO { get; set; }
        public DbSet<SMS_Sent_Details> SMS_Sent_Details { get; set; }
        public DbSet<SMS_Sent_Details_NowiseDMO> SMS_Sent_Details_NowiseDMO { get; set; }
        public DbSet<SMSApprovalStatusDMO> SMSApprovalStatusDMO { get; set; }
        public DbSet<Adm_M_Employee_StudentDMO> Adm_M_Employee_StudentDMO { get; set; }
        public DbSet<Adm_AdmissionCancelDMO> Adm_AdmissionCancelDMO { get; set; }
        public DbSet<VaccineAgeCriteriaDMO> VaccineAgeCriteriaDMO { get; set; }
        public DbSet<VaccineAgeCriteriaDetailsDMO> VaccineAgeCriteriaDetailsDMO { get; set; }
        public DbSet<Adm_StudentWise_Vaccine_DetailsDMO> Adm_StudentWise_Vaccine_DetailsDMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_SubjectsDMO { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Adm_M_Student>().HasKey(m => m.AMST_Id);
            builder.Entity<readmitstudentDMO>().HasKey(m => m.ARS_Id);
            builder.Entity<School_Adm_Y_StudentDMO>().HasKey(m => m.ASYST_Id);
            builder.Entity<StudentUserLoginDMO>().HasKey(m => m.IVRMSTUUL_Id);
            builder.Entity<StudentUserLogin_Institutionwise>().HasKey(m => m.IVRMSTUULI_Id);
            builder.Entity<StudentAppUserLoginDMO>().HasKey(m => m.IVRMUSLAPP_ID);
            builder.Entity<MasterStudentBondDMO>().HasKey(m => m.AMSTB_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            //updateUpdatedProperty<Adm_M_Student>();
            //updateUpdatedProperty<School_Adm_Y_StudentDMO>();

            return base.SaveChanges();
        }

        //private void updateUpdatedProperty<T>() where T : class
        //{
        //    var modifiedSourceInfo =
        //        ChangeTracker.Entries<T>()
        //            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        //    foreach (var entry in modifiedSourceInfo)
        //    {
        //        //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
        //    }
        //}
    }
}
