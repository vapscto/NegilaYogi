using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_Students_Certificate_Apply")]
   public class Adm_Students_Certificate_Apply_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASCA_CertificateType { get; set; }
        public string ASCA_Reason { get; set; }
        public DateTime ASCA_ApplyDate { get; set; }
        public string ASCA_Status { get; set; }
        public bool ASCA_ActiveFlg { get; set; }

        public List<Adm_Students_Certificate_Approve_DMO> Adm_Students_Certificate_Approve_DMO { get; set; }


    }
}
