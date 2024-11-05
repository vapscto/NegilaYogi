using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_SMS_MAIL_PARAMETER")]
    public class SMS_MAIL_PARAMETER_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMP_ID { get; set; }
        public string ISMP_NAME { get; set; }
        public string ISMP_Query { get; set; }
        public string ISMP_ParameterDesc { get; set; }

    }
}
