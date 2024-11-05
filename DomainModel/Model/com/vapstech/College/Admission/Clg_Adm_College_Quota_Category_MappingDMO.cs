using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Quota_Category_Mapping", Schema = "CLG")]
    public class Clg_Adm_College_Quota_Category_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACQCM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ACQ_Id { get; set; }
        public long ACQC_Id { get; set; }

        public bool ACQCM_ActiveFlg { get; set; }
    }
}
