using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Deactivate", Schema = "CLG")]
    public class CollegeActiveDeactiveStudentsReasonDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSDE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public string ACSDE_DeactivatedReason { get; set; }
        public DateTime? ACSDE_DeactivatedDate { get; set; }
        public bool ACSDE_ActivedFlg { get; set; }
        public DateTime? ACSDE_ActivatedDate { get; set; }
        public string ACSDE_ActivatedReason { get; set; }
        public bool ACSDE_ActiveFlag { get; set; }
        public long ACSDE_CreatedBy { get; set; }
        public long ACSDE_UpdatedBy { get; set; }
    }
}
