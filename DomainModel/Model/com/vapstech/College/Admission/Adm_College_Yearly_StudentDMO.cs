using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Yearly_Student", Schema = "CLG")]
    public class Adm_College_Yearly_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACYST_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ACYST_RollNo { get; set; }
        public bool AYST_PassFailFlag { get; set; }
        public long LoginId { get; set; }
        public DateTime? ACYST_DateTime { get; set; }
        public int ACYST_ActiveFlag { get; set; }

    }
}
