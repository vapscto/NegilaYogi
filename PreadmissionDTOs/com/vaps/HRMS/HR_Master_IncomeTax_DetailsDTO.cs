using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_IncomeTax_DetailsDTO:CommonParamDTO
    {
        public long HRMITD_Id { get; set; }
        public long HRMIT_Id { get; set; }
        public decimal? HRMITD_AmountFrom { get; set; }
        public decimal? HRMITD_AmountTo { get; set; }
        public decimal? HRMITD_IncomeTax { get; set; }
        public bool HRMITD_ActiveFlag { get; set; }
        public long MI_Id { get; set; }
        public Array incomeTaxDetailsList { get; set; }
        public Array incomeTaxList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
      public string HRMIT_AgeFlag { get; set; }

       //public string CreatedDate { get; set; }
        public Array incomeTax { get; set; }

        public string IVRMMG_GenderName { get; set; }

        public long HRMIT_GenderFlag { get; set; }
    }
}
