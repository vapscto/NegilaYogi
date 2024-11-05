using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.Portals.Principal
{
    [Table("IVRM_Email_sentBox")]
    public class IVRM_Email_sentBoxDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMESB_ID { get; set; }
        public long MI_Id { get; set; }
        public string Email_Id { get; set; }
        public string Message { get; set; }
        public DateTime Datetime { get; set; }
        public string Message_id { get; set; }
        public string Module_Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


    }

}
