using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeYearlyReportDTO 
    {
        public long MI_Id { get; set; }
        public string HRES_Month { get; set; }
        public long roleId { get; set; }
        public long HRME_Id { get; set; }
        public string HRES_Year { get; set; }
        //institution
        public string HRME_EmployeeFirstName { get; set; }

        public string HRME_EmployeeMiddleName { get; set; }

        public string HRME_EmployeeLastName { get; set; }
        public Double? HRES_WorkingDays { get; set; }
        public DateTime? HRES_FromDate { get; set; }
        public DateTime? HRES_ToDate { get; set; }
        public string EmployeeName { get; set; }
        public decimal? LOPDays { get; set; }

        public string HRME_EmployeeCode { get; set; }
        public long HRES_Id { get; set; }
        public InstitutionDTO institutionDetails { get; set; }
        //designation name
        public string DesignationName { get; set; }
        //list for employee
        public MasterEmployeeDTO currentemployeeDetails { get; set; }
        //employee name dropdown
        public Array employeedropdown { get; set; }
        //employee Qualification
        public Array employequalification { get; set; }
        public long? HRMD_Id { get; set; }
        public int? HRME_EmployeeOrder { get; set; }

        public string HRMDES_Designationname { get; set; }

        public string HRMG_GradeName { get; set; }
        public int? HRMG_ORDER { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public decimal? empGrossSal { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }

        public Array leaveyeardropdown { get; set; }
        public long LogInUserId { get; set; }
        public EmployeeProfileReportDTO[] ArrayempsList { get; set; }
        public Array institutionDetails_Array { get; set; }
        public List<EmployeeProfileReportDTO> AllInOne { get; set; }
        public HR_Employee_Salary_DetailsDTO[] earningresult { get; set; }

        public HR_Employee_Salary_DetailsDTO[] arrearresult { get; set; }

        public HR_Employee_Salary_DetailsDTO[] deductionresult { get; set; }

        public decimal? grossEarning { get; set; }

        public decimal? grossArrear { get; set; }
        public decimal? grossDeduction { get; set; }
        public decimal? netSalary { get; set; }
        public Array employeeSalaryslipDetails { get; set; }
        public decimal? grosspayhead { get; set; }
        public decimal? HRESD_Amount { get; set; }

        public Array head { get; set; }
        public string HRME_Fromdate { get; set; }
        public string HRME_Todate { get; set; }
    }
}
