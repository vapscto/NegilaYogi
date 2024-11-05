using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeePunchDTO
    {
        public class Input
        {
            public long MI_Id { get; set; }
            public long HRME_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public DateTime? fromdate { get; set; }
            public DateTime? todate { get; set; }

        }
        public class Output
        {
           public string empFname { get; set; }
            public string empMname { get; set; }
            public string empLname { get; set; }
            public DateTime? HRME_DOJ { get; set; }
            public Array filldepartment { get; set; }
            public Array Emp_punchDetails { get; set; }
            public string HRMD_DepartmentName { get; set; }
            public string HRMDES_DesignationName { get; set; }

        }
        public class OutputD
        {
            public DateTime? punchdate { get; set; }
            public string punchtime { get; set; }
            public string InOutFlg { get; set; }

        }


        public Array EmployeePortal_PunchD { get; set; }
    }
}
