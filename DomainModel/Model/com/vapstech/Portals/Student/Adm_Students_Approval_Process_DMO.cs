using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_Students_Approval_Process")]
    public class Adm_Students_Approval_Process_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASAP_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASAP_ApprovalBy { get; set; }
        public bool ASAP_ActiveFlg { get; set; }
        public DateTime? ASAP_ApprovalDate { get; set; }


        public List<Adm_Students_Approval_Process_ClassSec_DMO> Adm_Students_Approval_Process_ClassSec_DMO { get; set; }

    }
}
