using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_IncomeTax_Details_CessDTO
    {
        public long HRMITDC_Id { get; set; }
        public long HRMITD_Id { get; set; }
        public long HRMITC_Id { get; set; }
        public decimal HRMITDC_Amount { get; set; }
        public bool HRMITDC_ActiveFlag { get; set; }
        public Array incomeTaxDetailsCessList { get; set; }
        public Array incomeTaxList { get; set; }
        public Array incomeTaxDetailsList { get; set; }
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public Array cessnamedropdown { get; set; }


        public long HRMIT_Id { get; set; }

        //public bool HRMITDC_ActiveFlag { get; set; }

        //public decimal HRMITDC_Amount { get; set; }
       public string HRMIT_AgeFlag { get; set; }
        public string HRMITC_CessName { get; set; }
        public string CreatedDate { get; set; }
    }
}
