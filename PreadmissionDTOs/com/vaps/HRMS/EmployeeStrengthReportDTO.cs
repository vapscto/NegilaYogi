using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeStrengthReportDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public long? HRME_Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FormatType { get; set; }

        public bool AllEmployee { get; set; }
        public bool Departmentwise { get; set; }
        public bool LeftEmployee { get; set; }



        public Array employeedropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }


        public long?[] groupTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }

        public Array employeeDetails { get; set; }


        //table data
        public string grouptypeName { get; set; }
        public string departmentName { get; set; }
        public string designationName { get; set; }

        public long totalEmployees { get; set; }
        public long totalLeftEmployees { get; set; }
        public long totalWorkingEmployees { get; set; }

        public InstitutionDTO institutionDetails { get; set; }
        public long LogInUserId { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }
    }
}
