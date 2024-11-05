using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmpPortalDTO
    {
        public class Input
        {
            public long HRME_ID { get; set; }
            public long MI_Id { get; set; }

            public string HRES_Year { get; set; }

            public string HRES_Month { get; set; }
        }
            public class Output
        {
            public string HRMLY_LeaveYear { get; set; }

            public long HRME_Id { get; set; }
            public long hres_id { get; set; }
            public string monthName { get; set; }
            
            public decimal? salary { get; set; }

            public Array salarylist { get; set; }

            public Array salarylistD { get; set; }
            public Array salarylistE { get; set; }
            public Array totalDeduction { get; set; }
               public string earningname { get; set; }           
            public Array TotalEarning { get; set; }
        }
        public class Output_E_D
        {
            public decimal? hrmed_Amount { get; set; }
            public string hrmed_Name { get; set; }
        }

        public Array EmployeePortal_SalaryD { get; set; }
    }
}
