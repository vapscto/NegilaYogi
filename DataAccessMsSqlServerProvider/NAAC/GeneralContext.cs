using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.HRMS;
using DomainModel.Model.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Admission.Criteria7;
using DomainModel.Model.NAAC.Admission.Criteria8;
using DomainModel.Model.NAAC.Dental;
using DomainModel.Model.NAAC.Documents;
using DomainModel.Model.NAAC.HRMS;
using DomainModel.Model.NAAC.Medical;
using DomainModel.Model.NAAC.University;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.NAAC
{
    public class GeneralContext : DbContext
    {
        public GeneralContext(DbContextOptions<GeneralContext> options) : base(options)
        { }


        //==================================== COMMON DMO ====================================// 
        public DbSet<NAAC_Master_CycleDMO> NAAC_Master_CycleDMO { get; set; }
        public DbSet<NAAC_Master_Cycle_YearDMO> NAAC_Master_Cycle_YearDMO { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<NAAC_Master_Trust_CycleDMO> NAAC_Master_Trust_CycleDMO { get; set; }
        public DbSet<NAAC_Master_Trust_Cycle_MappingDMO> NAAC_Master_Trust_Cycle_MappingDMO { get; set; }
        public DbSet<NAAC_AC_Criteria_MarksSlab_DMO> NAAC_AC_Criteria_MarksSlab_DMO { get; set; }
        public DbSet<NAAC_User_PrivilegeDMO> NAAC_User_PrivilegeDMO { get; set; }
        public DbSet<NAAC_User_Privilege_SLDMO> NAAC_User_Privilege_SLDMO { get; set; }
        public DbSet<NAAC_User_Privilege_InstitutionDMO> NAAC_User_Privilege_InstitutionDMO { get; set; }
        public DbSet<NaacDocumentUploadDMO> NaacDocumentUploadDMO { get; set; }
        public DbSet<NAAC_Criteria_ApprovalDMO> NAAC_Criteria_ApprovalDMO { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<NAAC_AC_Criteria_General_FilesDMO> NAAC_AC_Criteria_General_FilesDMO { get; set; }
        public DbSet<NAAC_AC_Criteria_General_LinkDMO> NAAC_AC_Criteria_General_LinkDMO { get; set; }
        public DbSet<NAAC_AC_Criteria_GeneralDMO> NAAC_AC_Criteria_GeneralDMO { get; set; }
        public DbSet<IVRM_Master_Subjects_Branch_DMO> IVRM_Master_Subjects_Branch_DMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<PA_College_Student_SubjectMarks> PA_College_Student_SubjectMarks { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<MasterReligionDMO> Religion { get; set; }
        public DbSet<CollegemastercasteDMO> Caste { get; set; }
        public DbSet<CollegecastecaegoryDMO> CasteCategory { get; set; }
        public DbSet<MasterReference> MasterReference { get; set; }
        public DbSet<MasterSource> MasterSource { get; set; }
        public DbSet<MasterActivityDMO> MasterActivityDMO { get; set; }
        public DbSet<CollegeDocumentMasterDMO> MasterDocumentDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<AdmCourseBranchSemesterMappingDMO> AdmCourseBranchSemesterMappingDMO { get; set; }
        public DbSet<ClgMasterCategoryDMO> mastercategory { get; set; }
        public DbSet<ClgMasterCourseCategoryMapDMO> ClgMasterCourseCategorycategoryMap { get; set; }
        public DbSet<Adm_Course_Branch_MappingDMO> ClgMasterCourseBranchMap { get; set; }
        public DbSet<Adm_Course_Branch_MappingDMO> Adm_Course_Branch_MappingDMO { get; set; }
        public DbSet<IVRM_College_ReportDMO> IVRM_College_ReportDMO { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<AdmCollegeStudentSMSNoDMO> AdmCollegeStudentSMSNoDMO { get; set; }
        public DbSet<AdmCollegeStudentEmailIdDMO> AdmCollegeStudentEmailIdDMO { get; set; }
        public DbSet<AdmCollegeStudentParentsEmailIdDMO> AdmCollegeStudentParentsEmailIdDMO { get; set; }
        public DbSet<AdmCollegeStudentParentsMobileNoDMO> AdmCollegeStudentParentsMobileNoDMO { get; set; }
        public DbSet<MasterBorad> MasterBorad { get; set; }
        public DbSet<MasterSchoolType> MasterSchoolType { get; set; }
        public DbSet<AdmCollegeStudentReferenceDMO> AdmCollegeStudentReferenceDMO { get; set; }
        public DbSet<AdmCollegeStudentSourceDMO> AdmCollegeStudentSourceDMO { get; set; }
        public DbSet<AdmCollegeStudentPrevSchoolDMO> AdmCollegeStudentPrevSchoolDMO { get; set; }
        public DbSet<AdmCollegeStudentGuardianDMO> AdmCollegeStudentGuardianDMO { get; set; }
        public DbSet<AdmCollegeStudentSiblingsDetailsDMO> AdmCollegeStudentSiblingsDetailsDMO { get; set; }
        public DbSet<AdmCollegeStudentDocumentsDMO> AdmCollegeStudentDocumentsDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_UserDMO> Adm_College_Atten_Login_UserDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_DetailsDMO> Adm_College_Atten_Login_DetailsDMO { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Adm_College_Attendance_BatchDMO> Adm_College_Attendance_BatchDMO { get; set; }
        public DbSet<Adm_College_Atten_Batch_SubjectsDMO> Adm_College_Atten_Batch_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Atten_Batch_Subject_StudentsDMO> Adm_College_Atten_Batch_Subject_StudentsDMO { get; set; }
        public DbSet<Clg_Adm_College_Seat_DistributionDMO> Clg_Adm_College_Seat_DistributionDMO { get; set; }
        public DbSet<CLGAdm_College_RegNo_FormatDMO> CLGAdm_College_RegNo_FormatDMO { get; set; }
        public DbSet<Adm_M_Category> Adm_M_Category { get; set; }
        public DbSet<TT_Master_PeriodDMO> TT_Master_PeriodDMO { get; set; }
        public DbSet<CollegeAdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<Adm_Prv_Sch_CombinationDMO> Adm_Prv_Sch_CombinationDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Adm_College_Student_AttendanceDMO> Adm_College_Student_AttendanceDMO { get; set; }
        public DbSet<Adm_College_Student_Attendance_PeriodwiseDMO> Adm_College_Student_Attendance_PeriodwiseDMO { get; set; }
        public DbSet<Adm_College_Student_Attendance_StudentsDMO> Adm_College_Student_Attendance_StudentsDMO { get; set; }
        public DbSet<AdmissionRegisterDMO> AdmissionRegisterDMO { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<ApplUser> ApplUser { get; set; }
        public DbSet<CollegeStudentlogin> CollegeStudentlogin { get; set; }
        public DbSet<CollegeQuotaCourseBranchDocumentMappingDMO> CollegeQuotaCourseBranchDocumentMappingDMO { get; set; }
        public DbSet<BranchChangeDMO> BranchChangeDMO { get; set; }
        public DbSet<CollegeActiveDeactiveStudentsReasonDMO> CollegeActiveDeactiveStudentsReasonDMO { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<CollegeCancellationConfigurationDMO> CollegeCancellationConfigurationDMO { get; set; }
        public DbSet<Adm_College_Student_SubjectMarksDMO> Adm_College_Student_SubjectMarksDMO { get; set; }
        public DbSet<CollegeStudenttctransactionDMO> CollegeStudenttctransactionDMO { get; set; }
        public DbSet<MasterAcademic> Academic { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<Month> Month { get; set; }
        public DbSet<ClgMasterAcademicYearDMO> ClgMasterAcademicYearDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_School_Master_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Atten_Subject_MaxPeriodDMO> Adm_College_Atten_Subject_MaxPeriodDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<Clg_Adm_College_QuotaDMO> Clg_Adm_College_QuotaDMO { get; set; }
        public DbSet<Clg_Adm_College_Quota_CategoryDMO> Clg_Adm_College_Quota_CategoryDMO { get; set; }
        public DbSet<Clg_Adm_College_Quota_Category_MappingDMO> Clg_Adm_College_Quota_Category_MappingDMO { get; set; }
        public DbSet<AdmCollegeMasterBatchDMO> AdmCollegeMasterBatchDMO { get; set; }
        public DbSet<AdmCollegeSchemeTypeDMO> AdmCollegeSchemeTypeDMO { get; set; }
        public DbSet<AdmCollegeSubjectSchemeDMO> AdmCollegeSubjectSchemeDMO { get; set; }
        public DbSet<CLGAlumniUserRegistrationDMO> CLGAlumniUserRegistrationDMO { get; set; }
        public DbSet<CLGAlumni_M_StudentDMO> CLGAlumni_M_StudentDMO { get; set; }


        //================================== CRITERIA 1 =====================================//
        public DbSet<NAAC_AC_Master_Programs_112_DMO> NAAC_AC_Master_Programs_112_DMO { get; set; }
        public DbSet<NAAC_AC_Programs_112_DMO> NAAC_AC_Programs_112_DMO { get; set; }
        public DbSet<NAAC_AC_Programs_112_FilesDMO> NAAC_AC_Programs_112_FilesDMO { get; set; }
        public DbSet<NAAC_AC_TParticipation_113_DMO> NAAC_AC_TParticipation_113_DMO { get; set; }
        public DbSet<NAAC_AC_TParticipation_113_FilesDMO> NAAC_AC_TParticipation_113_FilesDMO { get; set; }
        public DbSet<NAAC_AC_SParticipation_123_DMO> NAAC_AC_SParticipation_123_DMO { get; set; }
        public DbSet<NAAC_AC_SParticipation_123_FilesDMO> NAAC_AC_SParticipation_123_FilesDMO { get; set; }
        public DbSet<NAAC_AC_SParticipation_123_Students_DMO> NAAC_AC_SParticipation_123_Students_DMO { get; set; }
        public DbSet<NAAC_MC_Master_Programs_112_DMO> NAAC_MC_Master_Programs_112_DMO { get; set; }
        public DbSet<NAAC_MC_Master_Programs_112_Details_DMO> NAAC_MC_Master_Programs_112_Details_DMO { get; set; }
        public DbSet<NAAC_MC_Master_Programs_112_Files_DMO> NAAC_MC_Master_Programs_112_Files_DMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_DMO> NAAC_AC_VAC_132_DMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_Comments_DMO> NAAC_AC_VAC_132_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_File_Comments_DMO> NAAC_AC_VAC_132_File_Comments_DMO { get; set; }


        public DbSet<NAAC_AC_VAC_132_Files_DMO> NAAC_AC_VAC_132_Files_DMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_Details_DMO> NAAC_AC_VAC_132_Details_DMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_Details_FilesDMO> NAAC_AC_VAC_132_Details_FilesDMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_Details_Comments_DMO> NAAC_AC_VAC_132_Details_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_Details_File_Comments_DMO> NAAC_AC_VAC_132_Details_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_VAC_132_Details_Students_DMO> NAAC_AC_VAC_132_Details_Students_DMO { get; set; }
        public DbSet<NAAC_AC_SProjects_133_DMO> NAAC_AC_SProjects_133_DMO { get; set; }
        public DbSet<NAAC_AC_SProjects_133_FilesDMO> NAAC_AC_SProjects_133_FilesDMO { get; set; }
        public DbSet<NAAC_MC_VAC_141_DMO> NAAC_MC_VAC_141_DMO { get; set; }
        public DbSet<NAAC_MC_VAC_142_DMO> NAAC_MC_VAC_142_DMO { get; set; }
        public DbSet<NAAC_MC_121_IntDept_CourseDMO> NAAC_MC_121_IntDept_CourseDMO { get; set; }
        public DbSet<NAAC_MC_121_IntDept_Course_FilesDMO> NAAC_MC_121_IntDept_Course_FilesDMO { get; set; }
        public DbSet<NAAC_MC_SProjects_134_DMO> NAAC_MC_SProjects_134_DMO { get; set; }
        public DbSet<NAAC_MC_SProjects_134_Files_DMO> NAAC_MC_SProjects_134_Files_DMO { get; set; }
        public DbSet<NAAC_HSU_InterdisciplinaryProgrammes_123_DMO> NAAC_HSU_InterdisciplinaryProgrammes_123_DMO { get; set; }
        public DbSet<NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO> NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO { get; set; }
        public DbSet<NAAC_HSU_Course_StaffMapping_122DMO> NAAC_HSU_Course_StaffMapping_122DMO { get; set; }
        public DbSet<NAAC_HSU_Course_StaffMapping_122_FilesDMO> NAAC_HSU_Course_StaffMapping_122_FilesDMO { get; set; }
        public DbSet<NAAC_AC_Programs_112_Comments_DMO> NAAC_AC_Programs_112_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_Programs_112_File_Comments_DMO> NAAC_AC_Programs_112_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_TParticipation_113_Comments_DMO> NAAC_AC_TParticipation_113_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_TParticipation_113_File_Comments_DMO> NAAC_AC_TParticipation_113_File_Comments_DMO { get; set; }
        public DbSet<NAAC_MC_121_IntDept_Course_Comments_DMO> NAAC_MC_121_IntDept_Course_Comments_DMO { get; set; }
        public DbSet<NAAC_MC_121_IntDept_Course_File_Comments_DMO> NAAC_MC_121_IntDept_Course_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_SParticipation_123_Comments_DMO> NAAC_AC_SParticipation_123_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_SParticipation_123_File_Comments_DMO> NAAC_AC_SParticipation_123_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_SProjects_133_Comments_DMO> NAAC_AC_SProjects_133_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_SProjects_133_File_Comments_DMO> NAAC_AC_SProjects_133_File_Comments_DMO { get; set; }

        public DbSet<NAAC_AC_512_InstScholarship_File_CommentsDMO> NAAC_AC_512_InstScholarship_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_512_InstScholarship_CommentsDMO> NAAC_AC_512_InstScholarship_CommentsDMO { get; set; }

        public DbSet<NAAC_MC_SProjects_134_File_Comments_DMO> NAAC_MC_SProjects_134_File_Comments_DMO { get; set; }
        public DbSet<NAAC_MC_SProjects_134_Comments_DMO> NAAC_MC_SProjects_134_Comments_DMO { get; set; }




        //====================================== CRITERIA 2 ===========================================//
        public DbSet<NAAC_MC_232_SKills_DMO> NAAC_MC_232_SKills_DMO { get; set; }
        public DbSet<NAAC_MC_254_CourseImprovement_DMO> NAAC_MC_254_CourseImprovement_DMO { get; set; }
        public DbSet<NAAC_MC_Measures_221_DMO> NAAC_MC_Measures_221_DMO { get; set; }
        public DbSet<NAAC_MC_EmpTrainedDevelopment244_DMO> NAAC_MC_EmpTrainedDevelopment244_DMO { get; set; }
        public DbSet<NAAC_MC_EmpTrainedDevelopment244_files_DMO> NAAC_MC_EmpTrainedDevelopment244_files_DMO { get; set; }
        public DbSet<NAAC_MC_EmpTrainedDevelopment_244_File_Comments_DMO> NAAC_MC_EmpTrainedDevelopment_244_File_Comments_DMO { get; set; }
        public DbSet<NAAC_MC_EmpTrainedDevelopment_244_Comments_DMO> NAAC_MC_EmpTrainedDevelopment_244_Comments_DMO { get; set; }
        public DbSet<NAAC_HSU_StudentLearningLevels_221_DMO> NAAC_HSU_StudentLearningLevels_221_DMO { get; set; }
        public DbSet<NAAC_HSU_ClinicalSkills_232_DMO> NAAC_HSU_ClinicalSkills_232_DMO { get; set; }
        public DbSet<NAAC_HSU_ExaminationManagement_255_DMO> NAAC_HSU_ExaminationManagement_255_DMO { get; set; }
        public DbSet<NAAC_HSU_StudentComplaints_252_DMO> NAAC_HSU_StudentComplaints_252_DMO { get; set; }
        public DbSet<NAAC_HSU_StudentComplaints_252_Files_DMO> NAAC_HSU_StudentComplaints_252_Files_DMO { get; set; }
        public DbSet<NAAC_HSU_EvaluationRelated_253_DMO> NAAC_HSU_EvaluationRelated_253_DMO { get; set; }
        public DbSet<NAAC_HSU_EvaluationRelated_253_Files_DMO> NAAC_HSU_EvaluationRelated_253_Files_DMO { get; set; }


        //================================== CRITERIA 3 =======================================//
        public DbSet<Naac_MOU_DMO> Naac_MOU_DMO { get; set; }
        public DbSet<NAAC_AC_352_MOU_Comments_DMO> NAAC_AC_352_MOU_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_352_MOU_File_Comments_DMO> NAAC_AC_352_MOU_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_343_SA_Students_Files_DMO> NAAC_AC_343_SA_Students_Files_DMO { get; set; }
        public DbSet<NAAC_AC_343_SA_Employee_Files_DMO> NAAC_AC_343_SA_Employee_Files_DMO { get; set; }
        public DbSet<NAAC_AC_Awards_342_Files_DMO> NAAC_AC_Awards_342_Files_DMO { get; set; }
        public DbSet<NAAC_AC_Awards_342_Comments_DMO> NAAC_AC_Awards_342_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_Awards_342_File_Comments_DMO> NAAC_AC_Awards_342_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_IPR_322_Files_DMO> NAAC_AC_IPR_322_Files_DMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_Files_DMO> NAAC_AC_344_ExtnActivities_Files_DMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_Students_Files_DMO> NAAC_AC_344_ExtnActivities_Students_Files_DMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_Comments_DMO> NAAC_AC_344_ExtnActivities_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_File_Comments_DMO> NAAC_AC_344_ExtnActivities_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_352_MOU_Files_DMO> NAAC_AC_352_MOU_Files_DMO { get; set; }
        public DbSet<NAAC_AC_331_Files_DMO> NAAC_AC_331_Files_DMO { get; set; }
        public DbSet<NAAC_AC_331_DMO> NAAC_AC_331_DMO { get; set; }
        public DbSet<NAAC_AC_Committee_Comments_DMO> NAAC_AC_Committee_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_Committee_Members_File_Comments_DMO> NAAC_AC_Committee_Members_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_Committee_File_Comments_DMO> NAAC_AC_Committee_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_Committee_Members_Comments_DMO> NAAC_AC_Committee_Members_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_331_Comments_DMO> NAAC_AC_331_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_331_File_Comments_DMO> NAAC_AC_331_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_IPR_322_DMO> NAAC_AC_IPR_322_DMO { get; set; }
        public DbSet<NAAC_AC_IPR_322_Comments_DMO> NAAC_AC_IPR_322_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_IPR_322_File_Comments_DMO> NAAC_AC_IPR_322_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_Awards_342_DMO> NAAC_AC_Awards_342_DMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_DMO> NAAC_AC_344_ExtnActivities_DMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_Students_DMO> NAAC_AC_344_ExtnActivities_Students_DMO { get; set; }
        public DbSet<NAAC_AC_Committee_DMO> NAAC_AC_Committee_DMO { get; set; }
        public DbSet<NAAC_AC__Committee_Members_DMO> NAAC_AC__Committee_Members_DMO { get; set; }
        //public DbSet<NAAC_AC_Committee_Comments_DMO> NAAC_AC_Committee_Comments_DMO { get; set; }
       // public DbSet<NAAC_AC_Committee_File_Comments_DMO> NAAC_AC_Committee_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_351_Linkage_DMO> NAAC_AC_351_Linkage_DMO { get; set; }
        public DbSet<NAAC_AC_351_Linkage_Files_DMO> NAAC_AC_351_Linkage_Files_DMO { get; set; }
        public DbSet<NAAC_AC_351_Linkage_Comments_DMO> NAAC_AC_351_Linkage_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_351_Linkage_File_Comments_DMO> NAAC_AC_351_Linkage_File_Comments_DMO { get; set; }
        public DbSet<NAAC_HSU_345_TeacherResearchPapers_DMO> NAAC_HSU_345_TeacherResearchPapers_DMO { get; set; }
        public DbSet<NAAC_HSU_345_TeacherResearchPapers_Files_DMO> NAAC_HSU_345_TeacherResearchPapers_Files_DMO { get; set; }
        public DbSet<HSU_346_EMPApprovedJournalList_DMO> HSU_346_EMPApprovedJournalList_DMO { get; set; }
        public DbSet<HSU_346_EmpApprovedJournalLists_Files_DMO> HSU_346_EmpApprovedJournalLists_Files_DMO { get; set; }
        public DbSet<UC_312_TeachersResearchDMO> UC_312_TeachersResearchDMO { get; set; }
        public DbSet<UC_312_TeachersResearchFilesDMO> UC_312_TeachersResearchFilesDMO { get; set; }
        public DbSet<MC_314_ResearchAssociatesDMO> MC_314_ResearchAssociatesDMO { get; set; }
        public DbSet<MC_314_ResearchAssociates_FilesDMO> MC_314_ResearchAssociates_FilesDMO { get; set; }
        public DbSet<MC_342_HRIncentivesDMO> MC_342_HRIncentivesDMO { get; set; }
        public DbSet<MC_342_HRIncentives_FilesDMO> MC_342_HRIncentives_FilesDMO { get; set; }
        public DbSet<HSU_341_EthicsDMO> HSU_341_EthicsDMO { get; set; }
        public DbSet<NAAC_AC_343_StudentActivities_DMO> NAAC_AC_343_StudentActivities_DMO { get; set; }
        public DbSet<NAAC_AC_343_StudentActivities_Files_DMO> NAAC_AC_343_StudentActivities_Files_DMO { get; set; }
        public DbSet<MC_343_TechnologyTransferredDMO> MC_343_TechnologyTransferredDMO { get; set; }
        public DbSet<MC_343_TechnologyTransferred_FilesDMO> MC_343_TechnologyTransferred_FilesDMO { get; set; }
        public DbSet<NAAC_HSU_315_FacilitesDMO> NAAC_HSU_315_FacilitesDMO { get; set; }
        public DbSet<NAAC_HSU_316_Dept_AwardsDMO> NAAC_HSU_316_Dept_AwardsDMO { get; set; }
        public DbSet<HSU_323_ResearchProjectsRatioDMO> HSU_323_ResearchProjectsRatioDMO { get; set; }
        public DbSet<HSU_323_ResearchProjectsRatio_FilesDMO> HSU_323_ResearchProjectsRatio_FilesDMO { get; set; }
        public DbSet<HSU_334_CampusStartUpsDMO> HSU_334_CampusStartUpsDMO { get; set; }
        public DbSet<HSU_334_CampusStartUps_FilesDMO> HSU_334_CampusStartUps_FilesDMO { get; set; }
        public DbSet<NAAC_HSU_316_Dept_Awards_FilesDMO> NAAC_HSU_316_Dept_Awards_FilesDMO { get; set; }
        public DbSet<NAAC_AC_Committee_Files_DMO> NAAC_AC_Committee_Files_DMO { get; set; }
        public DbSet<NAAC_AC_Committee_Members_files_DMO> NAAC_AC_Committee_Members_files_DMO { get; set; }
        public DbSet<NAAC_AC_343_SA_Students_DMO> NAAC_AC_343_SA_Students_DMO { get; set; }
        public DbSet<NAAC_AC_343_SA_Employee_DMO> NAAC_AC_343_SA_Employee_DMO { get; set; }
        public DbSet<HSU_352_RevenueGeneratedDMO> HSU_352_RevenueGeneratedDMO { get; set; }
        public DbSet<HSU_352_RevenueGenerated_FilesDMO> HSU_352_RevenueGenerated_FilesDMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_Staff_DMO> NAAC_AC_344_ExtnActivities_Staff_DMO { get; set; }
        public DbSet<NAAC_AC_344_ExtnActivities_Staff_Files_DMO> NAAC_AC_344_ExtnActivities_Staff_Files_DMO { get; set; }
        public DbSet<NAAC_MC_351_CollaborationActivitiesDMO> NAAC_MC_351_CollaborationActivitiesDMO { get; set; }
        public DbSet<NAAC_MC_351_CollaborationActivities_FilesDMO> NAAC_MC_351_CollaborationActivities_FilesDMO { get; set; }

        //======================================= CRITERIA 4 ==============================================//
        public DbSet<NAAC_MC_424_Infrastructure_DMO> NAAC_MC_424_Infrastructure_DMO { get; set; }
        // comment

        public DbSet<NAAC_AC_413_ICT_Comments_DMO> NAAC_AC_413_ICT_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_413_ICT_File_Comments_DMO> NAAC_AC_413_ICT_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_413_ICT_DMO> NAAC_AC_413_ICT_DMO { get; set; }
        public DbSet<NAAC_AC_413_ICT_FilesDMO> NAAC_AC_413_ICT_FilesDMO { get; set; }
        public DbSet<NAAC_AC_414_Budget_DMO> NAAC_AC_414_Budget_DMO { get; set; }

        public DbSet<NAAC_AC_414_Budget_Comments_DMO> NAAC_AC_414_Budget_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_414_Budget_File_Comments_DMO> NAAC_AC_414_Budget_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_423_Memberships_DMO> NAAC_AC_423_Memberships_DMO { get; set; }
        public DbSet<NAAC_AC_423_Memberships_Comments_DMO> NAAC_AC_423_Memberships_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_423_Memberships_File_Comments_DMO> NAAC_AC_423_Memberships_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_424_Expenditure_DMO> NAAC_AC_424_Expenditure_DMO { get; set; }
        public DbSet<NAAC_AC_424_Expenditure_Comments_DMO> NAAC_AC_424_Expenditure_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_424_Expenditure_File_Comments_DMO> NAAC_AC_424_Expenditure_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_434_EContent_DMO> NAAC_AC_434_EContent_DMO { get; set; }
        public DbSet<NAAC_AC_434_EContent_Comments_DMO> NAAC_AC_434_EContent_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_441_ExpAcaFacility_DMO> NAAC_AC_441_ExpAcaFacility_DMO { get; set; }
        public DbSet<NAAC_AC_441_ExpAcaFacility_Comments_DMO> NAAC_AC_441_ExpAcaFacility_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_414_Budget_Files_DMO> NAAC_AC_414_Budget_Files_DMO { get; set; }
        public DbSet<NAAC_AC_423_Memberships_Files_DMO> NAAC_AC_423_Memberships_Files_DMO { get; set; }
        public DbSet<NAAC_AC_434_EContent_Files_DMO> NAAC_AC_434_EContent_Files_DMO { get; set; }
        public DbSet<NAAC_AC_434_EContent_File_Comments_DMO> NAAC_AC_434_EContent_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_441_ExpAcaFacility_Files_DMO> NAAC_AC_441_ExpAcaFacility_Files_DMO { get; set; }
        public DbSet<NAAC_AC_441_ExpAcaFacility_File_Comments_DMO> NAAC_AC_441_ExpAcaFacility_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_424_Expenditure_Files_DMO> NAAC_AC_424_Expenditure_Files_DMO { get; set; }
        public DbSet<NAAC_HSU_Accreditation_424DMO> NAAC_HSU_Accreditation_424DMO { get; set; }
        public DbSet<NAAC_MC_422_Clinical_Laboratory_DMO> NAAC_MC_422_Clinical_Laboratory_DMO { get; set; }
        public DbSet<NAAC_MC_422_Clinical_Laboratory_files_DMO> NAAC_MC_422_Clinical_Laboratory_files_DMO { get; set; }
        public DbSet<NAAC_MC_436_EContent_FilesDMO> NAAC_MC_436_EContent_FilesDMO { get; set; }
        public DbSet<NAAC_MC_436_EContentDMO> NAAC_MC_436_EContentDMO { get; set; }
        public DbSet<NAAC_MC_423_StuLearningResourceDMO> NAAC_MC_423_StuLearningResourceDMO { get; set; }
        public DbSet<NAAC_MC_423_StuLearningResource_FilesDMO> NAAC_MC_423_StuLearningResource_FilesDMO { get; set; }
        public DbSet<Naac_MC_IctFacility441_DMO> Naac_MC_IctFacility441_DMO { get; set; }
        public DbSet<Naac_MC_IctFacility441_filesDMO> Naac_MC_IctFacility441_filesDMO { get; set; }
        public DbSet<NAAC_MC_443_BandWidth_RangeDMO> NAAC_MC_443_BandWidth_RangeDMO { get; set; }
      




        //====================================== CRITERIA 5 ==========================================//
        public DbSet<NAAC_HSU_511_NonGovScholarship_File_CommentsDMO> NAAC_HSU_511_NonGovScholarship_File_CommentsDMO { get; set; }
        public DbSet<NAAC_HSU_511_NonGovScholarship_CommentsDMO> NAAC_HSU_511_NonGovScholarship_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_523_QualExams_File_CommentsDMO> NAAC_AC_523_QualExams_File_CommentsDMO { get; set; }

        public DbSet<NAAC_AC_523_QualExams_CommentsDMO> NAAC_AC_523_QualExams_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_531_SportsCA_Students_CommentsDMO> NAAC_AC_531_SportsCA_Students_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_531_SportsCA_File_CommentsDMO> NAAC_AC_531_SportsCA_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_521_Placement_File_CommentsDMO> NAAC_AC_521_Placement_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_521_Placement_CommentsDMO> NAAC_AC_521_Placement_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_533_SportsCA_Activities_CommentsDMO> NAAC_AC_533_SportsCA_Activities_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_533_SportsCA_Activities_File_CommentsDMO> NAAC_AC_533_SportsCA_Activities_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_522_HrEducation_File_CommentsDMO> NAAC_AC_522_HrEducation_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_522_HrEducation_CommentsDMO> NAAC_AC_522_HrEducation_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_516_GRI_CommentsDMO> NAAC_AC_516_GRI_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_542_AlumniCont_File_CommentsDMO> NAAC_AC_542_AlumniCont_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_542_AlumniCont_CommentsDMO> NAAC_AC_542_AlumniCont_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_543_AlumniMeetings_CommentsDMO> NAAC_AC_543_AlumniMeetings_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_543_AlumniMeetings_File_CommentsDMO> NAAC_AC_543_AlumniMeetings_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_516_GRI_File_CommentsDMO> NAAC_AC_516_GRI_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_515_VET_File_CommentsDMO> NAAC_AC_515_VET_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_515_VET_CommentsDMO> NAAC_AC_515_VET_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_514_CompExams_CommentsDMO> NAAC_AC_514_CompExams_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_514_CompExams_File_CommentsDMO> NAAC_AC_514_CompExams_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_511_GovScholarshipDMO> NAAC_AC_511_GovScholarshipDMO { get; set; }
        public DbSet<NAAC_AC_511_GovScholarshipFilesDMO> NAAC_AC_511_GovScholarshipFilesDMO { get; set; }
        public DbSet<NAAC_AC_512_InstScholarshipDMO> NAAC_AC_512_InstScholarshipDMO { get; set; }
        public DbSet<NAAC_AC_512_InstScholarshipFilesDMO> NAAC_AC_512_InstScholarshipFilesDMO { get; set; }
        public DbSet<NAAC_AC_513_DevSchemesDMO> NAAC_AC_513_DevSchemesDMO { get; set; }
        public DbSet<NAAC_AC_513_DevSchemeFilesDMO> NAAC_AC_513_DevSchemeFilesDMO { get; set; }
        public DbSet<NAAC_AC_514_CompExamsDMO> NAAC_AC_514_CompExamsDMO { get; set; }
        public DbSet<NAAC_AC_514_CompExamsFilesDMO> NAAC_AC_514_CompExamsFilesDMO { get; set; }
        public DbSet<NAAC_AC_515_VETDMO> NAAC_AC_515_VETDMO { get; set; }
        public DbSet<NAAC_AC_515_VETFilesDMO> NAAC_AC_515_VETFilesDMO { get; set; }
        public DbSet<NAAC_AC_516_GRIDMO> NAAC_AC_516_GRIDMO { get; set; }
        public DbSet<NAAC_AC_516_GRIFilesDMO> NAAC_AC_516_GRIFilesDMO { get; set; }
        public DbSet<NAAC_AC_521_PlacementDMO> NAAC_AC_521_PlacementDMO { get; set; }
        public DbSet<NAAC_AC_521_PlacementFilesDMO> NAAC_AC_521_PlacementFilesDMO { get; set; }
        public DbSet<NAAC_AC_522_HrEducationDMO> NAAC_AC_522_HrEducationDMO { get; set; }
        public DbSet<NAAC_AC_522_HrEducationFilesDMO> NAAC_AC_522_HrEducationFilesDMO { get; set; }
        public DbSet<NAAC_AC_523_QAMastersDMO> NAAC_AC_523_QAMastersDMO { get; set; }
        public DbSet<NAAC_AC_523_QualExamsDMO> NAAC_AC_523_QualExamsDMO { get; set; }
        public DbSet<NAAC_AC_523_QualExamsFilesDMO> NAAC_AC_523_QualExamsFilesDMO { get; set; }
        public DbSet<NAAC_AC_533_SportsCA_ActivitiesDMO> NAAC_AC_533_SportsCA_ActivitiesDMO { get; set; }
        public DbSet<NAAC_AC_533_SportsCA_ActivitiesFilesDMO> NAAC_AC_533_SportsCA_ActivitiesFilesDMO { get; set; }
        public DbSet<NAAC_AC_542_AlumniContFilesDMO> NAAC_AC_542_AlumniContFilesDMO { get; set; }
        public DbSet<NAAC_AC_542_AlumniContDMO> NAAC_AC_542_AlumniContDMO { get; set; }
        public DbSet<NAAC_AC_543_AlumniMeetingsDMO> NAAC_AC_543_AlumniMeetingsDMO { get; set; }
        public DbSet<NAAC_AC_543_AlumniMeetingsFilesDMO> NAAC_AC_543_AlumniMeetingsFilesDMO { get; set; }
        public DbSet<NAAC_AC_531_SportsCADMO> NAAC_AC_531_SportsCADMO { get; set; }
        public DbSet<NAAC_AC_531_SportsCA_StudentsDMO> NAAC_AC_531_SportsCA_StudentsDMO { get; set; }
        public DbSet<NAAC_AC_531_SportsCAFilesDMO> NAAC_AC_531_SportsCAFilesDMO { get; set; }
        public DbSet<NAAC_HSU_511_NonGovScholarshipDMO> NAAC_HSU_511_NonGovScholarshipDMO { get; set; }
        public DbSet<NAAC_HSU_511_NonGovScholarship_FilesDMO> NAAC_HSU_511_NonGovScholarship_FilesDMO { get; set; }
        public DbSet<NAAC_AC_511_GovScholarship_CommentsDMO> NAAC_AC_511_GovScholarship_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_511_GovScholarship_File_CommentsDMO> NAAC_AC_511_GovScholarship_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_513_DevSchemes_File_CommentsDMO> NAAC_AC_513_DevSchemes_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_513_DevSchemes_CommentsDMO> NAAC_AC_513_DevSchemes_CommentsDMO { get; set; }


        //=========================================== CRITERIA 6 ===============================================//
        public DbSet<NAAC_AC_623_EGovernance_DMO> NAAC_AC_623_EGovernance_DMO { get; set; }
        public DbSet<NAAC_AC_632_FinanceSupport_DMO> NAAC_AC_632_FinanceSupport_DMO { get; set; }
        public DbSet<NAAC_AC_623_EGovernance_Files_DMO> NAAC_AC_623_EGovernance_Files_DMO { get; set; }
        public DbSet<NAAC_AC_632_FinanceSupport_Files_DMO> NAAC_AC_632_FinanceSupport_Files_DMO { get; set; }
        public DbSet<NAAC_AC_633_AdmTraining_DMO> NAAC_AC_633_AdmTraining_DMO { get; set; }
        public DbSet<NAAC_AC_634_DevPrograms_DMO> NAAC_AC_634_DevPrograms_DMO { get; set; }
        public DbSet<NAAC_AC_642_Funds_DMO> NAAC_AC_642_Funds_DMO { get; set; }
        public DbSet<NAAC_AC_653_IQAC_DMO> NAAC_AC_653_IQAC_DMO { get; set; }
        public DbSet<NAAC_AC_654_QualityAssurance_DMO> NAAC_AC_654_QualityAssurance_DMO { get; set; }
        public DbSet<NAAC_AC_633_AdmTraining_files_DMO> NAAC_AC_633_AdmTraining_files_DMO { get; set; }
        public DbSet<NAAC_AC_634_DevPrograms_files_DMO> NAAC_AC_634_DevPrograms_files_DMO { get; set; }
        public DbSet<NAAC_AC_642_Funds_files_DMO> NAAC_AC_642_Funds_files_DMO { get; set; }
        public DbSet<NAAC_AC_653_IQAC_files_DMO> NAAC_AC_653_IQAC_files_DMO { get; set; }
        public DbSet<NAAC_AC_654_QualityAssurance_files_DMO> NAAC_AC_654_QualityAssurance_files_DMO { get; set; }
        public DbSet<NAAC_AC_633_AdmTraining_Comments_DMO> NAAC_AC_633_AdmTraining_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_633_AdmTraining_File_Comments_DMO> NAAC_AC_633_AdmTraining_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_634_DevPrograms_Comments_DMO> NAAC_AC_634_DevPrograms_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_634_DevPrograms_File_Comments_DMO> NAAC_AC_634_DevPrograms_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_642_Funds_Comments_DMO> NAAC_AC_642_Funds_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_642_Funds_File_Comments_DMO> NAAC_AC_642_Funds_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_653_IQAC_Comments_DMO> NAAC_AC_653_IQAC_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_653_IQAC_File_Comments_DMO> NAAC_AC_653_IQAC_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_654_QualityAssurance_Comments_DMO> NAAC_AC_654_QualityAssurance_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_654_QualityAssurance_File_Comments_DMO> NAAC_AC_654_QualityAssurance_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_623_EGovernance_Comments_DMO> NAAC_AC_623_EGovernance_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_623_EGovernance_File_Comments_DMO> NAAC_AC_623_EGovernance_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_632_FinanceSupport_Comments_DMO> NAAC_AC_632_FinanceSupport_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_632_FinanceSupport_File_Comments_DMO> NAAC_AC_632_FinanceSupport_File_Comments_DMO { get; set; }


        //=========================================== CRITERIA 7 ============================================//
        public DbSet<NAAC_AC_711_GenderEquityDMO> NAAC_AC_711_GenderEquityDMO { get; set; }
        public DbSet<NAAC_AC_711_GenderEquity_CommentsDMO> NAAC_AC_711_GenderEquity_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_711_GenderEquity_FilesDMO> NAAC_AC_711_GenderEquity_FilesDMO { get; set; }
        public DbSet<NAAC_AC_711_GenderEquity_File_CommentsDMO> NAAC_AC_711_GenderEquity_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_713_AlternateEnergyDMO> NAAC_AC_713_AlternateEnergyDMO { get; set; }
        public DbSet<NAAC_AC_713_AlternateEnergy_FilesDMO> NAAC_AC_713_AlternateEnergy_FilesDMO { get; set; }
        public DbSet<NAAC_AC_714_LEDBulbsDMO> NAAC_AC_714_LEDBulbsDMO { get; set; }
        public DbSet<NAAC_AC_714_LEDBulbs_FilesDMO> NAAC_AC_714_LEDBulbs_FilesDMO { get; set; }
        public DbSet<NAAC_AC_714_LEDBulbs_Comments_DMO> NAAC_AC_714_LEDBulbs_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_714_LEDBulbs_File_Comments_DMO> NAAC_AC_714_LEDBulbs_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_718_WasteManagementDMO> NAAC_AC_718_WasteManagementDMO { get; set; }
        public DbSet<NAAC_AC_718_WasteManagement_FilesDMO> NAAC_AC_718_WasteManagement_FilesDMO { get; set; }
        public DbSet<NAAC_AC_718_WasteManagement_Comments_DMO> NAAC_AC_718_WasteManagement_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_718_WasteManagement_File_Comments_DMO> NAAC_AC_718_WasteManagement_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_719_DifferentlyAbledDMO> NAAC_AC_719_DifferentlyAbledDMO { get; set; }
        public DbSet<NAAC_AC_719_DifferentlyAbled_FilesDMO> NAAC_AC_719_DifferentlyAbled_FilesDMO { get; set; }
        public DbSet<NAAC_AC_7110_LocationalAdvtgDMO> NAAC_AC_7110_LocationalAdvtgDMO { get; set; }
        public DbSet<NAAC_AC_7110_LocationalAdvtg_CommentsDMO> NAAC_AC_7110_LocationalAdvtg_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_7110_LocationalAdvtg_FilesDMO> NAAC_AC_7110_LocationalAdvtg_FilesDMO { get; set; }
        public DbSet<NAAC_AC_7110_LocationalAdvtg_File_CommentsDMO> NAAC_AC_7110_LocationalAdvtg_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_7111_LocalCommunityDMO> NAAC_AC_7111_LocalCommunityDMO { get; set; }
        public DbSet<NAAC_AC_7111_LocalCommunity_FilesDMO> NAAC_AC_7111_LocalCommunity_FilesDMO { get; set; }
        public DbSet<NAAC_AC_7111_LocalCommunity_CommentsDMO> NAAC_AC_7111_LocalCommunity_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_7111_LocalCommunity_File_CommentsDMO> NAAC_AC_7111_LocalCommunity_File_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_7112_CodeOfCoductDMO> NAAC_AC_7112_CodeOfCoductDMO { get; set; }
        public DbSet<NAAC_AC_7112_CodeOfCoduct_FilesDMO> NAAC_AC_7112_CodeOfCoduct_FilesDMO { get; set; }
        // comment
        public DbSet<NAAC_AC_7112_CodeOfCoduct_Comments_DMO> NAAC_AC_7112_CodeOfCoduct_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7112_CodeOfCoduct_File_Comments_DMO> NAAC_AC_7112_CodeOfCoduct_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7113_CoreValues_Comments_DMO> NAAC_AC_7113_CoreValues_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7113_CoreValues_File_Comments_DMO> NAAC_AC_7113_CoreValues_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7113_CoreValuesDMO> NAAC_AC_7113_CoreValuesDMO { get; set; }
        public DbSet<NAAC_AC_7113_CoreValues_FilesDMO> NAAC_AC_7113_CoreValues_FilesDMO { get; set; }
        public DbSet<NAAC_AC_7115_ProfessionalEthics_Comments_DMO> NAAC_AC_7115_ProfessionalEthics_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7115_ProfessionalEthics_File_Comments_DMO> NAAC_AC_7115_ProfessionalEthics_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7117_UniversalValuesDMO> NAAC_AC_7117_UniversalValuesDMO { get; set; }
        public DbSet<NAAC_AC_7117_UniversalValues_FilesDMO> NAAC_AC_7117_UniversalValues_FilesDMO { get; set; }
        public DbSet<NAAC_AC_7117_UniversalValues_Comments_DMO> NAAC_AC_7117_UniversalValues_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7117_UniversalValues_File_Comments_DMO> NAAC_AC_7117_UniversalValues_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7116_StatutoryBodiesDMO> NAAC_AC_7116_StatutoryBodiesDMO { get; set; }
        public DbSet<NAAC_AC_7116_StatutoryBodies_Comments_DMO> NAAC_AC_7116_StatutoryBodies_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7116_StatutoryBodies_File_Comments_DMO> NAAC_AC_7116_StatutoryBodies_File_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7116_StatutoryBodies_FilesDMO> NAAC_AC_7116_StatutoryBodies_FilesDMO { get; set; }

        public DbSet<NAAC_MC_715_WaterConservFacilities_Comments_DMO> NAAC_MC_715_WaterConservFacilities_Comments_DMO { get; set; }
        public DbSet<NAAC_AC_7115_ProfessionalEthicsDMO> NAAC_AC_7115_ProfessionalEthicsDMO { get; set; }
        public DbSet<NAAC_AC_7115_ProfessionalEthics_FilesDMO> NAAC_AC_7115_ProfessionalEthics_FilesDMO { get; set; }

        public DbSet<NAAC_AC_7114_HumanValuesDMO> NAAC_AC_7114_HumanValuesDMO { get; set; }
        public DbSet<NAAC_AC_7114_HumanValues_CommentsDMO> NAAC_AC_7114_HumanValues_CommentsDMO { get; set; }
        public DbSet<NAAC_AC_7114_HumanValues_FilesDMO> NAAC_AC_7114_HumanValues_FilesDMO { get; set; }
        public DbSet<NAAC_AC_7114_HumanValues_File_CommentsDMO> NAAC_AC_7114_HumanValues_File_CommentsDMO { get; set; }
        public DbSet<NAAC_MC_713_AlternateEnergyDMO> NAAC_MC_713_AlternateEnergyDMO { get; set; }
        public DbSet<NAAC_MC_715_WaterConservFacilitiesDMO> NAAC_MC_715_WaterConservFacilitiesDMO { get; set; }
        public DbSet<NAAC_MC_716_GreenCampusInitiativesDMO> NAAC_MC_716_GreenCampusInitiativesDMO { get; set; }
        public DbSet<NAAC_MC_717_DisabledFriendlyEnvironmentDMO> NAAC_MC_717_DisabledFriendlyEnvironmentDMO { get; set; }
        public DbSet<NAAC_MC_716_AuditOnEnvironmentDMO> NAAC_MC_716_AuditOnEnvironmentDMO { get; set; }
        public DbSet<NAAC_MC_716_AuditOnEnvironment_FilesDMO> NAAC_MC_716_AuditOnEnvironment_FilesDMO { get; set; }


        //=========================================== CRITERIA 8 ==============================================//
        public DbSet<NAAC_811MC_NEET_DMO> NAAC_811MC_NEET_DMO { get; set; }
        public DbSet<MC_811_NEET_CommentsDMO> MC_811_NEET_CommentsDMO { get; set; }
        public DbSet<MC_811_NEET_File_CommentsDMO> MC_811_NEET_File_CommentsDMO { get; set; }
        public DbSet<DC_8111_ExpenditureDMO> DC_8111_ExpenditureDMO { get; set; }
        public DbSet<DC_8111_Expenditure_CommentsDMO> DC_8111_Expenditure_CommentsDMO { get; set; }
        public DbSet<DC_8111_ExpenditureFilesDMO> DC_8111_ExpenditureFilesDMO { get; set; }
        public DbSet<DC_8111_Expenditure_File_CommentsDMO> DC_8111_Expenditure_File_CommentsDMO { get; set; }
        public DbSet<NC_818_EmpCommitteesDMO> NC_818_EmpCommitteesDMO { get; set; }
        public DbSet<NAAC_NC_818_EmpCommittees_CommentsDMO> NAAC_NC_818_EmpCommittees_CommentsDMO { get; set; }
        public DbSet<NC_818_EmpCommitteesFilesDMO> NC_818_EmpCommitteesFilesDMO { get; set; }
        public DbSet<NAAC_NC_818_EmpCommittees_File_CommentsDMO> NAAC_NC_818_EmpCommittees_File_CommentsDMO { get; set; }
        public DbSet<NAAC_811MC_NEET_Files_DMO> NAAC_811MC_NEET_Files_DMO { get; set; }
        public DbSet<NAAC_MC_813_PGDegrees_DMO> NAAC_MC_813_PGDegrees_DMO { get; set; }
        public DbSet<NAAC_MC_813_PGDegrees_CommentsDMO> NAAC_MC_813_PGDegrees_CommentsDMO { get; set; }
        public DbSet<NAAC_MC_813_PGDegrees_Files_DMO> NAAC_MC_813_PGDegrees_Files_DMO { get; set; }
        public DbSet<NAAC_MC_813_PGDegrees_File_CommentsDMO> NAAC_MC_813_PGDegrees_File_CommentsDMO { get; set; }
        public DbSet<NAAC_MC_8110_Immunisation_DMO> NAAC_MC_8110_Immunisation_DMO { get; set; }
        public DbSet<NAAC_MC_8110_Immunisation_CommentsDMO> NAAC_MC_8110_Immunisation_CommentsDMO { get; set; }
        public DbSet<NAAC_MC_8110_Immunisation_Files_DMO> NAAC_MC_8110_Immunisation_Files_DMO { get; set; }
        public DbSet<NAAC_MC_8110_Immunisation_File_CommentsDMO> NAAC_MC_8110_Immunisation_File_CommentsDMO { get; set; }
        public DbSet<MC_819_Accredition_ClinicallabDMO> MC_819_Accredition_ClinicallabDMO { get; set; }
        public DbSet<MC_819_ClinicalLaboratory_CommentsDMO> MC_819_ClinicalLaboratory_CommentsDMO { get; set; }
        public DbSet<DC_813_ClinicalTeachingDMO> DC_813_ClinicalTeachingDMO { get; set; }
        public DbSet<DC_815_EquipmentTrainingDMO> DC_815_EquipmentTrainingDMO { get; set; }
        public DbSet<DC_816_SpecializedClinicsDMO> DC_816_SpecializedClinicsDMO { get; set; }

        //*************** CONSOLIDATED PROCESS *************//
        public DbSet<HR_Employee_BOSBOEDMO> HR_Employee_BOSBOEDMO { get; set; }
        public DbSet<HR_Employee_BOSBOE_CommentsDMO> HR_Employee_BOSBOE_CommentsDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {

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
