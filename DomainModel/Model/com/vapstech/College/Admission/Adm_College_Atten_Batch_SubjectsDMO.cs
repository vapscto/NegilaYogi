using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Atten_Batch_Subjects", Schema = "CLG")]
    public class Adm_College_Atten_Batch_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACABS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ACAB_Id { get; set; }
        public int ACABS_StudentStrength { get; set; }
        public List<Adm_College_Atten_Batch_Subject_StudentsDMO> Detailslist { get; set; }
    }
}
