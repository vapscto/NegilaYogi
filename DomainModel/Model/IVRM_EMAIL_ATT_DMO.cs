using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Email_attachment")]
    public class IVRM_EMAIL_ATT_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRM_EA { get; set; }
        public long ISES_Id { get; set; }
        public string IVRM_Att_Name { get; set; }
        public string IVRM_Att_Path { get; set; }
    }
}
