using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_SMS_Email_Setting_RoleType")]
    public class SMS_Email_Setting_RoleTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long ISESRT_Id { get; set; }
        public long ISES_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public bool ISESRT_ActiveFlg { get; set; }
        public long? ISESRT_CreatedBy { get; set; }
        public DateTime ISESRT_CreatedDate { get; set; }
        public long? ISESRT_UpdatedBy { get; set; }
        public DateTime ISESRT_UpdatedDate { get; set; }
    }
}
