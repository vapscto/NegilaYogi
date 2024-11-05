using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeReportsDTO
    {
        public InstitutionDTO institutionDetails;

        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        //Type
        public int Type { get; set; }

        public string FormatType { get; set; }

        public string DOBJL { get; set; }

        public string AllOrIndividual { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
        public long? HRMET_Id { get; set; }

        public long? HRME_Id { get; set; }

        public string TypeOrEmployee { get; set; }

        public bool? Working { get; set; }
        public bool? Left { get; set; }



        public Array monthdropdown { get; set; }
        public Array employeedropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }



        public long?[] groupTypeselected { get; set; }
        public long?[] employeeTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }

        public MasterEmployeeDTO[] masterEmployeeList { get; set; }

        public Array employeeDetails { get; set; }

        public EmployeeReportsDTO[] headerselected { get; set; }

        public Array headerdropdown { get; set; }

        public string columnName { get; set; }
        public string columnID { get; set; }

        public string coloumns { get; set; }


        public Array employeeDetailsfromDatabase { get; set; }

        public string empIds { get; set; }
        public long LogInUserId { get; set; }

        public long?[] hrmgT_IdList { get; set; }

        public long?[] hrmD_IdList { get; set; }
    }
}
