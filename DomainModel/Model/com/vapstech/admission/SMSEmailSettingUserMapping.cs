using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("IVRM_SMS_Email_Setting_User")]
    public class SMSEmailSettingUserMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISESUSR_Id { get; set; }
        public long ISES_Id { get; set; }
        public long UserId { get; set; }
        public bool? ISESUSR_ActiveFlg { get; set; }
        public long? ISESUSR_CreatedBy { get; set; }
        public DateTime? ISESUSR_CreatedDate { get; set; }
        public long? ISESUSR_UpdatedBy { get; set; }
        public DateTime? ISESUSR_UpdatedDate { get; set; }
    }
}
