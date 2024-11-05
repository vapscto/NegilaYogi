using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SMSMasterApprovalDTO
    {
        public long SMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long SSD_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long user_id { get; set; }
        public long SSDNO_MobileNo { get; set; }
        public long SSDNO_Id { get; set; }
        public int SMA_Level { get; set; }
        public long IVRMSTAUL_Id { get; set; }
        public long SSD_TransactionId { get; set; }
        public string SMA_SMSMailCallFlag { get; set; }
        public bool SMA_ActiveFlag { get; set; }
        public string IVRMSTAUL_UserName { get; set; }
        public string SSDN_SMSMessage { get; set; }
        public Array userNamelist { get; set; }
        public bool returnval { get; set; }
        public bool otpapproveflag { get; set; }
        public Array smsemailaplist { get; set; }
        public Array editdata { get; set; }
        public Array smsdetailslist { get; set; }
        public Array mainlist { get; set; }
        public string dup { get; set; }
        public string headername { get; set; }
        public string retmsg { get; set; }
        public string STAD_ApprStatus { get; set; }
        public string smsStatus { get; set; }
        public DateTime? ASA_FromDate { get; set; }
        public DateTime SSD_SentDate { get; set; }
        public DateTime? ASA_ToDate { get; set; }
        public Array headernamelist { get; set; }
        public bool snd_sms { get; set; }
        public bool snd_email { get; set; }
        public bool snd_call { get; set; }
        public SMSMasterApprovalDTO[] listdata { get; set; }

    }
}
