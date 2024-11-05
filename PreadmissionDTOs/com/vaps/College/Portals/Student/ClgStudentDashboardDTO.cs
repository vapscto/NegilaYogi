using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals
{
    public class ClgStudentDashboardDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }        
        public long roleid { get; set; }
        public long? AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public int COEME_Id { get; set; }
        public string COEME_EventName { get; set; }
        public string COEEI_Images { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }
        public string ASMAY_Year { get; set; }
        public int month { get; set; }
        public Array yearlist { get; set; }
        public Array currentyear { get; set; }
        public Array studentdetails { get; set; }
        public Array attendancedetails { get; set; }
        public Array noticeboard { get; set; }
        public Array feedetails { get; set; }
        public Array coereportlist { get; set; }
        public Array calenderlist { get; set; }
        public Array librarydetails { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string amst_mobile { get; set; }
        public string amst_email_id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long user_id { get; set; }
        public Array hodlist { get; set; }
        public long IHOD_Id { get; set; }
        public bool IHOD_ActiveFlag { get; set; }
        public string IHOD_Flg { get; set; }
        public bool IHODS_ActiveFlag { get; set; }
        public Array saved_hods_stf { get; set; }
        public bool returnval { get; set; }
        public string returnsavestatus { get; set; }

        public ClgStudentDashboardDTO[] employee { get; set; }
        public Array saved_hods { get; set; }
        public Array branchlist { get; set; }
        public Array query01 { get; set; }
        public ClgStudentDashboardDTO[] class_lst { get; set; }
        public string branchname { get; set; }
        public bool IHODB_ActiveFlag { get; set; }
        public Array saved_hods_cls { get; set; }
        public Array hodbranch { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public Array Fillstudentstrenth { get; set; }
        public long userId { get; set; }
        public string roletype { get;set;}
        public string flag { get; set; }
        public long roleId { get; set; }

        public Array FillstudentstrenthEMP { get; set; }

        public Array studentbuspassdetails { get; set; }


        public Array salarylist { get; set; }
        public Array filldepartment { get; set; }
        public Array mobile { get; set; }
        public Array email { get; set; }
        public Array coedata { get; set; }

        public long INTB_Id { get; set; }
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public string INTB_Attachment { get; set; }
        public string INTB_FilePath { get; set; }
        public DateTime? INTB_DisplayDate { get; set; }
        public DateTime INTB_StartDate { get; set; }
        public DateTime INTB_EndDate { get; set; }
        public long ASMCL_Id { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public long IVRMRMAP_Id { get; set; }
        public string mobileprivileges { get; set; }

        public string student_staffflag { get; set; }
        public Array viewstudentjoineddetails { get; set; }


        public string studentname { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCO_CourseName { get; set; }
        public string ASMC_SectionName { get; set; }

        public string AMCST_Sex { get; set; }
        public string AMCST_SOL { get; set; }
        public DateTime AMCST_Date { get; set; }
        public DateTime? AMCST_DOB { get; set; }

        public Array viewstudentdetails { get; set; }
        public Array viewstudentacademicyeardetails { get; set; }
        public Array viewstudentguardiandetails { get; set; }
        public Array viewstudentattendancetails { get; set; }
        public string Status_Flag { get; set; }
        public int order { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public Array viewstudentattendanceMonthdetails { get; set; }
        public Array viewstudentsubjectdetails { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public long subjorder { get; set; }
        public bool ECSTSU_ElectiveFlag { get; set; }
        public Array viewstudentaddressdetails { get; set; }
        public string OnClickOrOnChange { get; set; }
        public string messag { get; set; }
        public Array noticelist { get; set; }
        public Array noticelist_byid { get; set; }
        public long AMAY_RollNo { get; set; }
    }
}
