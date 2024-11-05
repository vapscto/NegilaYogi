using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class Employee16DTO
    {
       
      
     
      
        public long MI_Id { get; set; }
      public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public Array leaveyeardropdown { get; set; }
       
     
       public long? HRME_Id { get; set; }


   
       
       // public string hrmE_EmployeeFirstName { get; set; }
      
        public string hrme_address { get; set; }
      //  public Array employee_id { get; set; }

   
        public Array empDetails { get; set; }
        public string HRME_PFAccNo { get; set; }
        public string HRME_FatherName { get; set; }
        public string HRME_PerCity { get; set; }
       public string HRMDES_DesignationName { get; set; }
        public Array designation { get; set; }
        public string HRME_PANCardNo { get; set; }

        public string HRME_EmployeeFirstName { get; set; }

        public InstitutionDTO institutionDetails;
        public long? HRME_PerPincode { get; set; }

    }
}
