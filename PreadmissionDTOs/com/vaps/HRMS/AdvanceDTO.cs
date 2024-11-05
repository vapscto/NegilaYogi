using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class AdvanceReportDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
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


        public Array groupTypedropdown { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }

        public string hrmE_EmployeeFirstName { get; set; }

        public string hrme_employeecode { get; set; }

        public string HRESA_AdvMonth { get; set; }

        public int HRESA_AdvYear { get; set; }

        public DateTime HRESA_EntryDate { get; set; }

        public decimal? HRESA_AppliedAmount { get; set; }


    }

}