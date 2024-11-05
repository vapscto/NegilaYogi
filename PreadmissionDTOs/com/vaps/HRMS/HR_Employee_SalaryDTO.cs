using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Employee_SalaryDTO
    {
        public long HRES_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long userid { get; set; }        
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public Double? abc { get; set; }
        public Double? HRES_WorkingDays { get; set; }
        public string HRES_DailyRates { get; set; }
        public decimal HRELT_TotDays { get; set; }
        public decimal? HRES_EPF { get; set; }
        public decimal? HRES_FPF { get; set; }
        public decimal? HRES_Ac21 { get; set; }
        public decimal? HRES_Ac22 { get; set; }
        public decimal? HRES_Ac5 { get; set; }
        public DateTime? HRES_FromDate { get; set; }
        public DateTime? HRES_ToDate { get; set; }
        public bool? HRES_ArrearRegFlag { get; set; }
        public string HRES_BankCashFlag { get; set; }
        public long? HRMGT_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string HRES_BankCode { get; set; }
        public string HRES_AccountNo { get; set; }
        public decimal? HRES_ESIEmplr { get; set; }
        public long HRME_Age { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public long basicamount { get; set; }

        public string check_role { get; set; }

        public long roleId { get; set; }
        public string retrunMsg { get; set; }
        public Array monthdropdown { get; set; }
        public Array employeetype { get; set; }
        public Array employeedropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }
        public Array employeedetailList { get; set; }

        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array ReportData { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long LogInUserId { get; set; }
        public MasterEmployeeDTO[] masterEmployeeList { get; set; }

        public Array employeeDetails { get; set; }

        public HR_ConfigurationDTO  configurationDetails {get;set;}


        public InstitutionDTO institutionDetails { get; set; }
        public MasterEmployeeDTO currentemployeeDetails { get; set; }

        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string GenderName { get; set; }
        public string HRME_FatherName { get; set; }

        public string NetAmountInwords { get; set; }

        public Array employeeSalaryslipDetails { get; set; }

        public Array groupTypedropdown { get; set; }

        public long?[] groupTypeIdList { get; set; }

        public CumulativeSalaryReportDTO CumulativeSalaryReportDTO { get; set; }
        public HR_Master_EarningsDeductionsDTO[] headList { get; set; }

        public decimal LopAmount { get; set; }
        public decimal Lopdays { get; set; }

        public bool? HRME_EPFNotApplicableFlg { get; set; }
        public bool? HRME_RetiredFlg { get; set; }
        public bool? HRME_FPFNotApplicableFlg { get; set; }


        public Array employeeLeaveDetails { get; set; }
        public HR_Employee_SalaryDTO empsaldetail { get; set; }
        public HR_ConfigurationDTO PayrollStandard { get; set; }

        public string EmailSMS { get; set; }

        public string hrmE_EmployeeCode { get; set; }

        public string TemplateString  { get; set; }

        public HR_Employee_SalaryDTO[] Template { get; set; }


        public Array employe { get; set; }
        public Array earningdeductiondetails { get; set; }

        public long[] empid { get; set; }

        public Array main_list { get; set; }

        public decimal? empGrossSal { get; set; }

        public decimal? HRESD_Amount { get; set; }

        public Array employeeSalaryslipapproveDetails { get; set; }
        public string comm { get; set; }
        public string columnnames { get; set; }
        public string totalworkingdays { get; set; }

        public Array filldata { get; set; }
        public string[] MonthList { get; set; }
        public decimal? PensionFund { get; set; }

        public class HR_Cumulative_Salary_Report
        {
            public string HRES_Year { get; set; }
            public long HRME_Id { get; set; }
            public string HRES_Month { get; set; }
            public Array employeeSalaryslipDetails { get; set; }
            public Array earningdetails { get; set; }
            public Array dectuiondetails { get; set; }
            public Array institutionDetails { get; set; }
            public long?[] hrmdeS_IdList { get; set; }
            public long MI_Id { get; set; }
            public long roleId { get; set; }
            public long LogInUserId { get; set; }
            public multimonthlist[] monthselected { get; set; }

        }

        public class multimonthlist
        {
            public string IVRM_Month_Id { get; set; }
            public string IVRM_Month_Name { get; set; }
        }

    }
   


    }
