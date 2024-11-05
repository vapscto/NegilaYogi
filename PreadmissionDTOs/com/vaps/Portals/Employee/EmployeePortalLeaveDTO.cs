using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeePortalLeaveDTO
    {
       
            public class Input
            {
                public long MI_Id { get; set; }
                public long HRME_Id { get; set; }
                public long HRMLY_Id { get; set; }
                

            }
            public class Output
            {
              // public string HRME_EmployeeFirstName { get; set; }

            public string HRML_LeaveName { get; set; }

            public decimal HRELS_TotalLeaves { get; set; }
            public decimal HRELS_TransLeaves { get; set; }
            public decimal HRELS_CBLeaves { get; set; }
           

            }
        public class OutputA
        {
            public Array leaveList { get; set; }
        }
         public Array EmployeePortal_LeaveD { get; set; }
      }
}
