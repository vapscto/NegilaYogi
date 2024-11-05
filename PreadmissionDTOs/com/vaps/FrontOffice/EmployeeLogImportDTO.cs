using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
  public  class EmployeeLogImportDTO
    {
      
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public long HRME_Id { get; set; }
            //public long HRMET_Id { get; set; }
            public bool returnval { get; set; }
            public string message { get; set; }
            public Array employeedetails { get; set; }

            public EMPDataEMPORT[] empDataimport { get; set; }

        }


        public class EMPDataEMPORT
        {

            public string EmployeeFirstName { get; set; }
            public string EmployeeCode { get; set; }
            public string PunchIN_Time { get; set; }
            public string PunchOut_Time { get; set; }
            public string PunchDate { get; set; }

        }
    
}
