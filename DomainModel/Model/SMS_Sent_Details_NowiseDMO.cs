using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("SMS_Sent_Details_Nowise")]
    public class SMS_Sent_Details_NowiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SSDNO_Id { get; set; }
        public long SSD_Id { get; set; }
        public long SSDNO_MobileNo { get; set; }
        public int SSDNO_NoOfAttempt { get; set; }
        public string SSDNO_Status { get; set; }
        public DateTime? SSDNO_DeliveryDate { get; set; }
        public string SSD_DeliveryTime { get; set; }
        public string SSDNO_OutboxFlag { get; set; }
        public string SSDNO_ApprovalLevel { get; set; }

        public string SSDN_SMSMessage { get; set; }

        public string SSDNO_SMSStatusId { get; set; }

     
    }
}
