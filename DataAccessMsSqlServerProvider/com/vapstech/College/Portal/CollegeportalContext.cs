using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.COE;
using DomainModel.Model.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.Portals.Chairman;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.HOD;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Portal
{
    public class CollegeportalContext : DbContext
    {
        public CollegeportalContext(DbContextOptions<CollegeportalContext> options) : base(options)
        { Database.SetCommandTimeout(30000); }
     
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
       public DbSet<MasterAcademic> academicYearDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<IVRM_NoticeBoard_Student_College_ViewedDMO> IVRM_NoticeBoard_Student_College_ViewedDMO { get; set; }
        public DbSet<IVRM_NoticeBoard_FilesDMO> IVRM_NoticeBoard_FilesDMO { get; set; }
        public DbSet<MobileApplAuthenticationDMO> MobileApplAuthenticationDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_ExamsDMO> Exm_Col_Yearly_Scheme_ExamsDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_GroupDMO> Exm_Col_Yearly_Scheme_GroupDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_Group_SubjectsDMO> Exm_Col_Yearly_Scheme_Group_SubjectsDMO { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<Adm_College_Atten_Login_UserDMO> Adm_College_Atten_Login_UserDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_DetailsDMO> Adm_College_Atten_Login_DetailsDMO { get; set; }
        public DbSet<Institution> Institution_master { get; set; }
        public DbSet<IVRM_MobileApp_Download_DMO> IVRM_MobileApp_Download_DMO { get; set; }
        public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }
        public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        public DbSet<COE_Events_ImagesDMO> COE_Events_Images { get; set; }
        public DbSet<Exm_Col_Studentwise_SubjectsDMO> Exm_Col_Studentwise_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<exammasterDMO> exammasterDMO { get; set; }      
        public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Student_Leave_ApplyDMO> Adm_College_Student_Leave_ApplyDMO { get; set; }
        public DbSet<Adm_College_Student_Leave_ApprovalDMO> Adm_College_Student_Leave_ApprovalDMO { get; set; }

        //================================Staff Portal
        public DbSet<HR_Master_LeaveYearDMO> HR_MasterLeaveYear { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<HR_Employee_Salary> HR_Employee_Salary { get; set; }
        public DbSet<HR_Employee_EarningsDeductions> HR_Employee_EarningsDeductions { get; set; }
        public DbSet<HR_Employee_Salary_Details> HR_Employee_Salary_Details { get; set; }
        public DbSet<HR_Master_EarningsDeductions> HR_Master_EarningsDeductions { get; set; }
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<Fee_Y_PaymentDMO> Fee_Y_PaymentDMO { get; set; }
        public DbSet<Fee_Y_Payment_College_StudentDMO> Fee_Y_Payment_College_StudentDMO { get; set; }
        public DbSet<CLG_Exm_Col_Student_Marks_Process_SubjectwiseDMO> CLG_Exm_Col_Student_Marks_Process_SubjectwiseDMO { get; set; }
        public DbSet<exammasterDMO> col_exammasterDMO { get; set; }
        public DbSet<IVRM_GalleryDMO> IVRM_GalleryDMO { get; set; }
        public DbSet<IVRM_Gallery_PhotosDMO> IVRM_Gallery_PhotosDMO { get; set; }
        public DbSet<IVRM_Gallery_VideosDMO> IVRM_Gallery_VideosDMO { get; set; }
        public DbSet<IVRM_Gallery_ProgramsDMO> IVRM_Gallery_ProgramsDMO { get; set; }
        //================================ Portal IVRM
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<IVRM_NoticeBoardDMO> IVRM_NoticeBoardDMO { get; set; }

        public DbSet<FeeGroupDMO> FeeGroupDMO { get; set; }
        public DbSet<FeeHeadDMO> FeeHeadsDMO { get; set; }
        
        public DbSet<IVRM_NoticeBoard_CoBranchDMO> IVRM_NoticeBoard_CoBranchDMO { get; set; }

        public DbSet<IVRM_NoticeBoard_Student_CollegeDMO> IVRM_NoticeBoard_Student_CollegeDMO { get; set; }
        public DbSet<IVRM_College_PN_StudentDMO> IVRM_College_PN_StudentDMO { get; set; }
        public DbSet<IVRM_PushNotificationDMO> IVRM_PushNotificationDMO { get; set; }
        public DbSet<Adm_College_Student_GFeedbackDMO> Adm_College_Student_GFeedbackDMO { get; set; }
        public DbSet<IVRM_HOD_Staff_DMO> IVRM_HOD_Staff_DMO { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_DesignationDMO { get; set; }


        //==============Interaction
        public DbSet<IVRM_Interactions_StudentDMO> IVRM_Interactions_StudentDMO { get; set; }
        public DbSet<IVRM_Interactions_Student_StaffDMO> IVRM_Interactions_Student_StaffDMO { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClass> Adm_SchoolAttendanceLoginUserClass { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUser> Adm_SchoolAttendanceLoginUser { get; set; }
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<IVRM_School_Master_InteractionsDMO> IVRM_School_Master_InteractionsDMO { get; set; }
        public DbSet<IVRM_School_Transaction_InteractionsDMO> IVRM_School_Transaction_InteractionsDMO { get; set; }


        // shilpa 
        public DbSet<ClgMasterCategoryDMO> ClgMasterCategoryDMO { get; set; }
        public DbSet<ClgMasterCourseCategoryMapDMO> ClgMasterCourseCategoryMapDMO { get; set; }


        //Akash
        public DbSet<IVRM_NoticeBoard_Staff_DMO> IVRM_NoticeBoard_Staff_DMO_con { get; set; }


        //EXAM
        public DbSet<Exm_Col_CourseBranchDMO> Exm_Col_CourseBranchDMO { get; set; }
        public DbSet<Exm_Col_Yearly_SchemeDMO> Exm_Col_Yearly_SchemeDMO { get; set; }
        public DbSet<Exm_Col_Yrly_Sch_Exams_SubwiseDMO> Exm_Col_Yrly_Sch_Exams_SubwiseDMO { get; set; }
        public DbSet<CLG_Exm_Col_Student_Marks_ProcessDMO> CLG_Exm_Col_Student_Marks_ProcessDMO { get; set; }
        //EXAM


        public DbSet<COE_Events_CourseBranchDMO> COE_Events_CourseBranchDMO { get; set; }
        public DbSet<COE_Events_EmployeesDMO> COE_Events_EmployeesDMO { get; set; }

        public DbSet<Adm_Students_Certificate_Apply_DMO> Adm_Students_Certificate_Apply_DMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<Adm_Students_Leave_Apply_DMO> Adm_Students_Leave_Apply_DMO { get; set; }
        public DbSet<IVRM_Month_DMO> IVRM_Month_DMO { get; set; }
        public DbSet<HOD_DMO> HOD_DMO { get; set; }
        public DbSet<IVRM_HOD_Branch_DMO> IVRM_HOD_Branch_DMO { get; set; }
        public DbSet<IVRM_sms_sentBoxDMO> IVRM_sms_sentBoxDMO { get; set; }
        public DbSet<IVRM_Email_sentBoxDMO> IVRM_Email_sentBoxDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<School_M_Class> admissioncls { get; set; }
        public DbSet<CollegeStudentlogin> CollegeStudentlogin { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }

    }
}
