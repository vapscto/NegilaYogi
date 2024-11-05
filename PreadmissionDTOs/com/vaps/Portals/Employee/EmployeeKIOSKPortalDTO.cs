using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeeKIOSKPortalDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_TechNonTeachingFlg { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_PhotoNo { get; set; }
        public string DeviceID { get; set; }
        public string HRME_EmailId { get; set; }
        public Array filldepartment { get; set; }
        public Array mobile { get; set; }
        public Array email { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public DateTime? HRME_DOB { get; set; }
    }

    public class EmployeeKioskLEAVEDTO
    {
        public DateTime HRELTD_FromDate { get; set; }
        public DateTime HRELT_ToDate { get; set; }
        public long MI_Id { get; set; }
        public string selectedEmployee { get; set; }
        public string selectedLeave { get; set; }
        public Array activityIds { get; set; }
    }

    //public class EmpKioskEmployeeDeatilsDTO
    //{
    //    public long MI_Id { get; set; }
    //    public long UserId { get; set; }
    //    public long HRME_Id { get; set; }
    //    public long HRMD_Id { get; set; }
    //    public string HRME_TechNonTeachingFlg { get; set; }
    //    public string HRME_EmployeeFirstName { get; set; }
    //    public string HRME_PhotoNo { get; set; }
    //    public Array filldepartment { get; set; }
    //    public DateTime? HRME_DOJ { get; set; }
    //    public DateTime? HRME_DOB { get; set; }
    //    public string HRME_EmployeeCode { get; set; }
    //    public string HRMD_DepartmentName { get; set; }
    //    public string HRMDES_DesignationName { get; set; }
    //    public Array mobile { get; set; }
    //    public Array email { get; set; }
    //}

    public class EmployeeKioskPunchDTO
    {
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public DateTime? punchdate { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public string punchtime { get; set; }
        public string InOutFlg { get; set; }
        public Array Emp_punchDetails { get; set; }
    }

    public class EmployeeKioskSalaryDTO
    {
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public DateTime? punchdate { get; set; }
        public long MI_Id { get; set; }
        public long hres_id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public string punchtime { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public string InOutFlg { get; set; }
        public string monthName { get; set; }
        public string HRES_Year { get; set; }
        public Array Emp_punchDetails { get; set; }
        public Array yearlist { get; set; }
        public Array salarylist { get; set; }
        public Array salaryEarningDlist { get; set; }
        public decimal? salary { get; set; }
    }

    public class EmployeeKioskTimeTableDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string P_Days { get; set; }
        public string Period { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array TT_final_generation { get; set; }
        public Array allperiods { get; set; }
        public Array periods { get; set; }
        public Array class_sectons { get; set; }
    }
}
