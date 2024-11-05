using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.BirthDay
{
    [Table("IVRM_SMS_Email_Setting_Parameter")]
    public class IVRM_SMS_Email_Setting_ParameterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISESP_Id { get; set; }
        public long ISES_Id { get; set; }
        public long ISMP_Id { get; set; }
    
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISESP_CreatedBy { get; set; }
        public long ISESP_UpdatedBy { get; set; }
    }
}
