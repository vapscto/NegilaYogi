using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class CTCReportDTO
    {

        public long HRES_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public Double? HRES_WorkingDays { get; set; }
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

    }
}
