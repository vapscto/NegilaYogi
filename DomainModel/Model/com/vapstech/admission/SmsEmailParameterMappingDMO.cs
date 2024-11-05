using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("IVRM_SMS_MAIL_PARAMETER_MAPPING")]
    public class SmsEmailParameterMappingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMPRM_Id { get; set; }
        public long IVRMHE_Id { get; set; }
        public long ISMP_ID { get; set; }
    }
}
