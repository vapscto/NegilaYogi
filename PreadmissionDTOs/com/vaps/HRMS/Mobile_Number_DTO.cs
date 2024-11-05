using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class Mobile_Number_DTO : CommonParamDTO
    {
        public long HRMEMNO_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        //public long hrmE_MobileNo { get; set; }
        public string HRMEMNO_DeFaultFlag { get; set; }
        // public string mobile_option { get; set; }
        public long PhoneNumber { get; set; }
        public long MI_Id { get; set; }
    }
}
