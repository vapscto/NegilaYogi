using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("SMS_Sent_Details")]
    public class SMS_Sent_Details : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SSD_Id { get; set; }
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
    }
}
