using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class ESIReportDTO
    {

        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }

        public Array monthdropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }


        public string HRME_EmployeeCode { get; set; }
        public long?[] groupTypeselected { get; set; }
        public long?[] employeeTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }

        public Array employeeDetails { get; set; }

        //datatable values

        public string HRME_ESIAccNo { get; set; }
        public string EmployeeName { get; set; }
        public Double? HRES_WorkingDays { get; set; }
        public decimal? EmployeeContibution { get; set; }
        public decimal? EmployerContibution { get; set; }
        public decimal? totalAmount { get; set; }
        public DateTime? HRME_DOL { get; set; }

        public decimal? basicAmount { get; set; }

        public InstitutionDTO institutionDetails { get; set; }

        public long LogInUserId { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public string HRC_ECodePrefix { get; set; }
        public HR_ConfigurationDTO configurationDetails { get; set; }
    }
}
