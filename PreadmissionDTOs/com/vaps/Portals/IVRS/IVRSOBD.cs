using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
   public class IVRSOBD
    {
        public long  MI_ID { get; set; }
        public long  ASMAY_ID { get; set; }
        public long  ASMCL_ID { get; set; }
        public long  ASMS_ID { get; set; }
        public Array maindata { get; set; }
        public string studentName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_emailId { get; set; }
        public long AMST_MobileNo { get; set; }
        public long AMST_Id { get; set; }
        public Array acs_lst { get; set; }
        public Array clas_list { get; set; }
        public Array sect_list { get; set; }
        public string schoolname { get; set; }

        public string ivrid { get; set; }

        public Temp_mobileDTO[] selected_list { get; set; }
        public string IMCS_VirtualNo { get; set; }
        public string IMCD_InOutFlg { get; set; }
        public string IMCD_CallStatus { get; set; }
        public string IMCD_CallDuration { get; set; }
        public long IMCD_PulseCount { get; set; }
        public string returnMsg { get; set; }

        public details[] Details { get; set; }
        public class Temp_mobileDTO
        {
            public long IVRS_MobileNo { get; set; }
        }

        public class details
        {
            public string StatusCode { get; set; }
        }
    }
}
