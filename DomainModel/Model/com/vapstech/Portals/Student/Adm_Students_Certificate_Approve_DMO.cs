using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_Students_Certificate_Approve")]
    public class Adm_Students_Certificate_Approve_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASCAP_Id { get; set; }
        public long ASCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMALU_Id { get; set; }
        public string ASCAP_Status { get; set; }
        public string ASCAP_ApproveReason { get; set; }
        public DateTime ASCAP_ApproveDate { get; set; }        
        public bool ASCAP_ActiveFlg { get; set; }

    }
}
