using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class SalaryDetailsDTO
    {
        public long HRES_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRES_Year { get; set; }
        public string multipledep { get; set; }
        public string multipledes { get; set; }
        public Array stafflist { get; set; }
        public string HRES_Month { get; set; }
        public Double? HRES_WorkingDays { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRES_DailyRates { get; set; }
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

        public long roleId { get; set; }
        public string retrunMsg { get; set; }
        public Array monthdropdown { get; set; }
        public Array employeedropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array employeeTypedropdown { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public MasterEmployeeDTO[] masterEmployeeList { get; set; }
        public Array employeeDetails { get; set; }
        public HR_ConfigurationDTO configurationDetails { get; set; }
        public InstitutionDTO institutionDetails { get; set; }
        public MasterEmployeeDTO currentemployeeDetails { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string NetAmountInwords { get; set; }
        public Array employeeSalaryslipDetails { get; set; }
        public Array groupTypedropdown { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public CumulativeSalaryReportDTO CumulativeSalaryReportDTO { get; set; }
        public HR_Master_EarningsDeductionsDTO[] headList { get; set; }
        public decimal LopAmount { get; set; }
        public Double? Lopdays { get; set; }
        public Array employeeLeaveDetails { get; set; }
        public HR_Employee_SalaryDTO empsaldetail { get; set; }
        public HR_ConfigurationDTO PayrollStandard { get; set; }

        public long HRC_Id { get; set; }       
        public decimal? HRC_PFMaxAmt { get; set; }
        public decimal? HRC_FPFPer { get; set; }
        public decimal? HRC_EPFPer { get; set; }
        public bool HRC_AsPerEmpFlag { get; set; }
        public string HRC_PFAccNoPrefix { get; set; }
        public decimal? HRC_AccNo2 { get; set; }
        public decimal? HRC_AccNo21 { get; set; }
        public decimal? HRC_AccNo22 { get; set; }
        public int HRC_RetirementYrs { get; set; }
        public string HRC_ECodePrefix { get; set; }
        public decimal? HRC_ESIMax { get; set; }
        public decimal? HRC_ESIEmplrCont { get; set; }
        public string HRC_PayMethodFlg { get; set; }
        public bool? HRC_ArrSalaryFlag { get; set; }
        public bool? HRC_CummArrFlag { get; set; }
        public int HRC_SalaryFromDay { get; set; }
        public int HRC_SalaryToDay { get; set; }

        public decimal? HRC_ESIMaxAmount { get; set; }
        public decimal? HRC_AC2MinAmount { get; set; }
        public decimal? HRC_AC21MinAmount { get; set; }
        public decimal? HRC_AC22MinAmount { get; set; }
       
        public string HRMGT_EmployeeGroupType { get; set; }
        public long?[] empids { get; set; }
        public string  serchtype { get; set; }
    }
}

