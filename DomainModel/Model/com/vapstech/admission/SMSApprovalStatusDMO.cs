using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("SMS_Transaction_Approval_Details")]
    public class SMSApprovalStatusDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long STAD_Id { get; set; }
        public long MI_Id { get; set; }
        public long STAD_TransNo { get; set; }
        public long IRMUL_Id { get; set; }
        public string STAD_ApprStatus { get; set; }
        public long? STAD_OTP { get; set; }
        public long SMA_Id { get; set; }

    }
}
