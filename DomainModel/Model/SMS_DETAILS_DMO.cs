using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_SMS_DETAILS")]
    public class SMS_DETAILS_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMSD_ID { get; set; }
        public long MI_ID { get; set; }
        public string IVRMSD_TYPE { get; set; }
        public string IVRMSD_DOMIN { get; set; }
        public string IVRMSD_URL { get; set; }
        public string IVRMSD_USERNAME { get; set; }
        public string IVRMSD_PASSWORD { get; set; }
        public string IVRMSD_SENDERID { get; set; }
        public string IVRMSD_ACTIVE { get; set; }
        public string IVRMSD_WORKINGKEY { get; set; }        
        public string IVRMSD_PD_Active { get; set; }
    }
}
