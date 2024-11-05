using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Chirman
{
    public class Emp_salaryDTO
    {

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMLY_Id { get; set; }

        
        public long HRMDES_Id { get; set; }
        public long hrmd_id { get; set; }
        public string designationname { get; set; }
        public string departmentname { get; set; }
        public Array leavedetails { get; set; }
        public Array masterleave { get; set; }
        public Array yearlist { get; set; }
        public Array deptsalary { get; set; }
        
        public decimal? salary { get; set; }

        public long HRME_Id { get; set; }
        public string empname { get; set; }
        public long HRML_Id { get; set; }
        public string HRML_LeaveName { get; set; }
        public string monthName { get; set; }
        
       // public long HRMLY_Id { get; set; }
        public int HRELS_TotalLeaves { get; set; }
        public int HRELS_TransLeaves { get; set; }
        public int HRELS_CBLeaves { get; set; }
        public double lop { get; set; }
        public DateTime? doj { get; set; }
        public Array fillmasterleave { get; set; }
        public Array designation { get; set; }
        public Array fillyear { get; set; }
        public Array department { get; set; }
        public Array salarylist { get; set; }
        public string HRMLY_LeaveYear { get; set; }
    public Array employeeDetails { get; set; }

        public long CL { get; set; }

        public long TL { get; set; }
        public long LT { get; set; }






    }
}


