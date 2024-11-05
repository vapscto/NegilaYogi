using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Birthday
{
    public class IVRM_sms_sentBoxDTO
    {
        public long IVRM_SSB_ID { get; set; }
        public long MI_Id { get; set; }
        public string Mobile_no { get; set; }
        public string Message { get; set; }
        public DateTime Datetime { get; set; }
        public string Message_id { get; set; }
        public string Module_Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string statusofmessage { get; set; }
        public string To_FLag { get; set; }
        public string System_Ip { get; set; }
        public string network_Ip { get; set; }
        public string MacAddress_Ip { get; set; }
        public int Schedular_Flag { get; set; }
    }
}
