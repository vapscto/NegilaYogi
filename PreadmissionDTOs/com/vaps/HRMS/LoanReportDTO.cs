using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class LoanReportDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public long IVRM_Month_Id { get; set; }
        public Array monthdropdown { get; set; }
        public InstitutionDTO institutionDetails { get; set; }

        public Array leaveyeardropdown { get; set; }
        public Array employeeSalaryslipDetails { get; set; }

        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long hrmE_Id { get; set; }

        public Array groupTypedropdown { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }

        public string hrmE_EmployeeFirstName { get; set; }

        public string hrme_employeecode { get; set; }

        public int HRELT_Year { get; set; }
        public string HRELT_Month { get; set; }

        public decimal? HREL_LoanAmount { get; set; }

        public decimal HRELT_LoanAmount { get; set; }
        public string HREL_LoanInsallments { get; set; }
        public string HRML_LoanType { get; set; }

        public decimal? HREL_TotalPending { get; set; }
    }

}