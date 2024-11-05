using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("SMS_Mail_Approval")]
    public class SMSMasterApprovalDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long SMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string SMA_SMSMailCallFlag { get; set; }
        public int SMA_Level { get; set; }
        public bool SMA_ActiveFlag { get; set; }
        public string SMA_HeaderName { get; set; }
    }
}
