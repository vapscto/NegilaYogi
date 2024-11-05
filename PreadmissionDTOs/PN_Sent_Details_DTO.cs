using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class PN_Sent_Details_DTO
    {
        public long PNSD_Id { get; set; }
        public long MI_Id { get; set; }
        public string PNSD_HeaderName { get; set; }
        public DateTime PNSD_SentDate { get; set; }
        public string PNSD_SentTime { get; set; }
        public string PNSD_TransactionId { get; set; }
        public string PNSD_ToFlag { get; set; }
        public string PNSD_SMSMessage { get; set; }
        public string PNSD_SystemIP { get; set; }
        public string PNSD_NetworkIP { get; set; }
        public string PNSD_MAACAddress { get; set; }
        public bool PNSD_SchedulerFlag { get; set; }
        public bool PNSD_OutboxFlag { get; set; }
        public long PNSD_ApprovalLevel { get; set; }
    }
}
