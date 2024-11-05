using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("IVRM_COLLEGE_REG_REPORT", Schema = "CLG")]
    public class AdmissionRegisterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRM_CLGREG_Id { get; set; }       
        public string IVRM_CLGREG_NAME { get; set; }
        public string IVRM_CLGREG_PAR { get; set; }        
    }
}
