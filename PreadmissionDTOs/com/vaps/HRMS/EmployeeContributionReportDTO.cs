using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeContributionReportDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public long? HRME_Id { get; set; }
        public string EarningDeduction { get; set; }
        public long? EarningHead { get; set; }
        public long? DeductionHead { get; set; }
        public string MonthBetweenDates { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FormatType { get; set; }

        public string hreS_Month { get; set; }
        public string hreS_Year { get; set; }


        public Array monthdropdown { get; set; }
        public Array employeedropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }

        public Array earningdropdown { get; set; }
        public Array detectiondropdown { get; set; }



        public long?[] groupTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }

        public Array employeeDetails { get; set; }
        public Array employeeContributionDetails { get; set; }
        public Array headerlist { get; set; }

        //table data
        public string EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string departmentName { get; set; }
        public string designationName { get; set; }
        public decimal? GrossSalary { get; set; }

        public decimal? selectedHeadAmount { get; set; }
        public string remarks { get; set; }

        public InstitutionDTO institutionDetails { get; set; }

        public long LogInUserId { get; set;}

        public long?[] hrmgT_IdList { get; set; }

        public long?[] hrmD_IdList { get; set; }


    }
}
