using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AdmissionSMSReportDTO
    {

        public string mobile { get; set; }
        public Array studentlist { get; set; }
        public Array smsdetails { get; set; }
        public Array moduledetails { get; set; }
        public long IVRMESB_ID { get; set; }
        public long IVRM_SSB_ID { get; set; }
        public long ISES_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMIM_Id { get; set; }
        public long IVRMM_Id { get; set; }

        public string ISES_Template_Name { get; set; }
        public string ISES_SMSMessage { get; set; }
        public bool ISES_SMSActiveFlag { get; set; }
        public string ISES_MailSubject { get; set; }
        public string ISES_MailBody { get; set; }
        public string ISES_MailFooter { get; set; }
        public string EmailId { get; set; }
        public string ISES_Mail_Message { get; set; }
        public string ISES_MailHTMLTemplate { get; set; }
        public bool ISES_MailActiveFlag { get; set; }
        public long IVRMSTAUL_Id { get; set; }
        public long IVRMIMP_Id { get; set; }
        // public string ISES_Parameter { get; set; }
        public Array emailSmsSettingList { get; set; }
        // public Array pageWiseHeaderList { get; set; }

        public int Module_ActiveFlag { get; set; }
        public Array parameterList { get; set; }
        public Array getdetails { get; set; }
        public Array Getreportdetails { get; set; }
        public long ISMP_ID { get; set; }
        public string ISMP_NAME { get; set; }
        public string Module_Name { get; set; }
        public DateTime DatetimeSMS { get; set; }
        public DateTime dailydate { get; set; }
        public string dailybtedates { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public DateTime Datetime { get; set; }
        public DateTime DatetimeEmail { get; set; }
        public string all2 { get; set; }
        public string message { get; set; }
        public Array pageWiseHeaderList { get; set; }
        public SMS_MAIL_PARAMETER_DTO[] templateParameter { get; set; }
        public bool returnval { get; set; }
        public object TempararyArrayheadList { get; set; }
        public string regornamedetails { get; set; }
        public string onclickloaddata { get; set; }
        public AdmissionSMSReportDTO[] messagelist { get; set; }
        public AdmissionSMSReportDTO[] smslist { get; set; }
        public long stud_count { get; set; }
        public string regorname { get; set; }
        public string[] mdata { get; set; }
    }
}
