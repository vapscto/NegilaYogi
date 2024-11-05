using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class massUpdationDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public string Type { get; set; }

        public long? HRME_Id { get; set; }

        public Array employeedropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array groupTypedropdown { get; set; }

        public Array headdropdown { get; set; }

        public Array eardettypelist { get; set; }


        public long?[] groupTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }

        //table data
        public string EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }


        //Form Data
        public long?[] employeeselected { get; set; }

        public string EarningDeduction { get; set; }
        public string AmountPercentage { get; set; }
        public decimal? HREED_Amount { get; set; }
        public string HREED_Percentage { get; set; }

        public long HRMED_Id { get; set; }

        public string AddRemove { get; set; }


        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array employeeDetails { get; set; }

        public long LogInUserId { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }
    }
}
