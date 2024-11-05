using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_SMS_MAIL_SAVED_PARAMETER_PAGEWISE")]
    public class SMS_MAIL_SAVED_PARAMETER_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSPP_ID { get; set; }
        public long ISMP_ID { get; set; }
        public long MI_Id { get; set; }
        public long ISES_Id { get; set; }
        public string Flag { get; set; }
    }
}
