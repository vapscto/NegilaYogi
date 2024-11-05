using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SMSResendDTO : CommonParamDTO
    {
        public long SSDNO_Id { get; set; }
        public long SSD_Id { get; set; }
        public long SSDNO_MobileNo { get; set; }
        public int SSDNO_NoOfAttempt { get; set; }
        public string transname { get; set; }
        public string SSDNO_Status { get; set; }
        public DateTime? SSDNO_DeliveryDate { get; set; }
        public string SSD_DeliveryTime { get; set; }
        public string SSDNO_OutboxFlag { get; set; }
        public string SSDNO_ApprovalLevel { get; set; }

        public string SSDN_SMSMessage { get; set; }

        public string SSDNO_SMSStatusId { get; set; }
        public long IVRMUL_Id { get; set; }
        public long MI_ID { get; set; }
        public string SSD_HeaderName { get; set; }
        public DateTime SSD_SentDate { get; set; }
        public TimeSpan SSD_Senttime { get; set; }
        public long SSD_TransactionId { get; set; }

        public string SSD_ToFlag { get; set; }
        //public string SSD_SMSMessage { get; set; }

        public string SSD_SystemIP { get; set; }

        public string varchar { get; set; }

        public string SSD_MAACAddress { get; set; }

        public bool SSD_SchedulerFlag { get; set; }

        public Array statuslist { get; set; }
        public Array headerlist { get; set; }
        public Array transnolist { get; set; }
        public Array fillgriddata { get; set; }
        public SMSResendDTO[] resenddata { get; set; }

        public  DateTime FromDate { get; set; }
        public  DateTime ToDate { get; set; }
        public string retmsg { get; set; }
        public string msgstatus { get; set; }
        public Array fillsentdata { get; set; }

        public string checkst { get; set; }
        public headdto[] headsname { get; set; }
        public transdto[] transnameid { get; set; }
        public stadto[] statusname { get; set; }

    }

    public class headdto
    {
        public string SSD_HeaderName { get; set; }

    }
    public class transdto
    {
        public long SSD_TransactionId { get; set; }

    }
    public class stadto
    {
        public string SSDNO_Status { get; set; }

    }
}
