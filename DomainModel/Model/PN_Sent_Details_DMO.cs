using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model
{
    [Table("PN_Sent_Details")]
    public class PN_Sent_Details_DMO
       
    {
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PNSD_Id { get; set; }
        public long MI_Id { get; set; }
        public string PNSD_HeaderName { get; set; }
        public DateTime PNSD_SentDate { get; set; }
        public string PNSD_SentTime { get; set; }
        public string PNSD_TransactionId { get; set; }
        public string PNSD_ToFlag { get; set; }
        public string PNSD_SMSMessage { get; set; }
        public string PNSD_Header_Flg { get; set; }
        public string PNSD_SystemIP { get; set; }
        public string PNSD_NetworkIP { get; set; }
        public string PNSD_MAACAddress { get; set; }
        public bool PNSD_SchedulerFlag { get; set; }
        public bool PNSD_OutboxFlag { get; set; }
        public string PNSD_ApprovalLevel { get; set; }

        public List<PN_Sent_Details_Devicewise_DMO> PN_Sent_Details_Devicewise_DMO { get; set; }
        public List<PN_Sent_Details_Staff_DMO> PN_Sent_Details_Staff_DMO { get; set; }
        public List<PN_Sent_Details_Student_DMO> PN_Sent_Details_Student_DMO { get; set; }
    }
}
