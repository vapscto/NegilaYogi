using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Birthday
{
    [Table("ivrm_email_sentbox")]
    public class ivrm_email_sentbox:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMESB_ID { get; set; }
        public long MI_Id { get; set; }
        public string Email_Id { get; set; }
        public string Message { get; set; }
        public DateTime? Datetime { get; set; }
        public string Message_id { get; set; }
        public string Module_Name { get; set; }
        public string To_FLag { get; set; }


    }
}
