using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HeaderwiseReportDTO
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

        public long?[] groupTypeselected { get; set; }
        public long?[] employeeTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }

        public Array employeeDetails { get; set; }

        //datatable columns 

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public decimal? TotalEarning { get; set; }
        public decimal? TotalDeduction { get; set; }
        public decimal? NetSalary { get; set; }
        public InstitutionDTO institutionDetails { get; set; }

        public long LogInUserId { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }
    }
}
