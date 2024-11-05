using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Chirman
{
    public class PAYCAREEmployeeDetailsDTO
    {

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long hrmd_id { get; set; }
        public string designationname { get; set; }
        public string departmentname { get; set; }
        public long? mobileno { get; set; }
        public string email { get; set; }
        public string mstatus { get; set; }
        public string gender { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public string empname { get; set; }
        public int empcount { get; set; }
        public long? HRME_Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? doj { get; set; }
        public DateTime? ToDate { get; set; }
        public string FormatType { get; set; }
        public int depttotalEmployees { get; set; }
        public bool? AllEmployee { get; set; }
        public bool? Departmentwise { get; set; }
        public bool? LeftEmployee { get; set; }


        
                public Array departmentgraph { get; set; }
        public Array employeedropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array filldesiganation { get; set; }

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

    }
}


