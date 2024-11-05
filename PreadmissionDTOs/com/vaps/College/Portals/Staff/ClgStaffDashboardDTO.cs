using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Portals.Staff
{
    public class ClgStaffDashboardDTO
    {
        public int IVRMSTAUL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long PaymentNootificationCollegeStaff { get; set; }
        public Array getpaymentnotificationdetails { get; set; }
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long roleid { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public long IVRMRMAP_Id { get; set; }
        public string mobileprivileges { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public string HRMEM_EmailId { get; set; }
        public Array calenderlist { get; set; }
        public Array coereportlist { get; set; }
         public Array coereportlist1 { get; set; }
        public Array employeedetails { get; set; }
        public Array empmobileno { get; set; }
        public Array empemailid { get; set; }
        public Array noticelist { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array fillstudent { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string amcst_FirstName { get; set; }
        public Array fillstudentalldetails { get; set; }
        public Array examlist { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMCST_FatherName { get; set; }
        public string AMCST_MotherName { get; set; }
        public string AMCST_BloodGroup { get; set; }
        public string AMCST_PerStreet { get; set; }
        public string AMCST_PerArea { get; set; }
        public string AMCST_PerCity { get; set; }
        public long? AMCST_FatherMobleNo { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public string AMCST_Sex { get; set; }
        public string AMCST_emailId { get; set; }
        public string ASMAY_Year { get; set; }
        public DateTime? AMCST_DOB { get; set; }
        public string EME_ExamName { get; set; }
        public string ECSTMP_Result { get; set; }
        public decimal ECSTMP_TotalMaxMarks { get; set; }
        public decimal ECSTMP_TotalObtMarks { get; set; }
        public decimal ECSTMP_Percentage { get; set; }
        public Array filldepartment { get; set; }
        public Array Emp_punchDetails { get;set; }
        public string empFname{ get; set; }
        public string empMname { get; set; }
        public string empLname { get; set; }
        public string HRMD_DepartmentName{ get; set; }
        public string HRMDES_DesignationName { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public DateTime? punchdate { get; set; }
        public string punchtime { get; set; }
        public string InOutFlg { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public long studentcount { get; set; }
    }
}

