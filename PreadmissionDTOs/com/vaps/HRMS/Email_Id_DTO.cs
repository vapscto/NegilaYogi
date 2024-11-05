using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class Email_Id_DTO:CommonParamDTO
    {
        public long HRMEEM_Id { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMEM_EmailId { get; set; }
        //public string hrmE_EmailId { get; set; }
        public string HRMEM_DeFaultFlag { get; set; }
        // public string email_option { get; set; }
    }
}
