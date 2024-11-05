using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("IVRM_College_Report", Schema = "CLG")]
    public class IVRM_College_ReportDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRM_CLG_Id { get; set; }
        public string IVRM_CLG_NAME { get; set; }
        public string IVRM_CLG_PAR { get; set; }
    }
}
