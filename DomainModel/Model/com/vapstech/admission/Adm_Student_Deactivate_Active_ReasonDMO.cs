using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Deactivate")]
    public class Adm_Student_Deactivate_Active_ReasonDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASDE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASDE_DeactivatedReason { get; set; }
        public DateTime? ASDE_DeactivatedDate { get; set; }
        public bool ASDE_ActivedFlg { get; set; }
        public DateTime? ASDE_ActivatedDate { get; set; }
        public string ASDE_ActivatedReason { get; set; }
        public bool ASDE_ActiveFlag{get;set;}
        public long ASDE_CreatedBy { get; set; }
        public long ASDE_UpdatedBy { get; set; }
    }
}
