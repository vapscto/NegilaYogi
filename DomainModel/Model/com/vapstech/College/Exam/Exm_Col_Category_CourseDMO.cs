using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_CourseBranch", Schema = "CLG")]
    public class Exm_Col_CourseBranchDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ECCCB_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long EMG_Id { get; set; }
        public long ACST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ECCCB_ActiveFlg { get; set; }

    }
}
