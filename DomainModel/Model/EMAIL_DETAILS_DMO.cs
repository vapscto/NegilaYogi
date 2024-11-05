using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_EMAIL_DETAILS")]
    public class EMAIL_DETAILS_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMD_ID { get; set; }
        public long MI_ID { get; set; }
        public string IVRMMD_NAME { get; set; }
        public bool IVRMMD_SERVICE { get; set; }
        public int IVRMMD_STU_TYPE { get; set; }
        public string IVRMMD_Mail_ID { get; set; }

        public string IVRMMD_Mail_PASSWORD { get; set; }
        public string IVRMMD_HOSTNAME { get; set; }

        public string IVRMMD_PORTNO { get; set; }

        public string IVRMMD_SUBJECT { get; set; }

        public string IVRMMD_Attechement { get; set; }

        public string IVRM_sendgridkey { get; set; }

        public string IVRM_mailcc { get; set; }
    }
}
