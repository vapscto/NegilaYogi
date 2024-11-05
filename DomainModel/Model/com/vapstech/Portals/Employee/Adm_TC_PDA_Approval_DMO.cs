using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Adm_TC_PDA_Approval")]
    public class Adm_TC_PDA_Approval_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ATCPDAAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime ATCPDAAPP_ApprovedDate { get; set; }
        public long ATCPDAAPP_ApprovedBy { get; set; }
        public bool ATCPDAAPP_ApprovalFlg { get; set; }
        public string ATCPDAAPP_Remarks { get; set; }
        public bool ATCPDAAPP_ActiveFlg { get; set; }
        public DateTime? ATCPDAAPP_CreatedDate { get; set; }
        public DateTime? ATCPDAAPP_UpdatedDate { get; set; }
        public long? ATCPDAAPP_CreatedBy { get; set; }
        public long? ATCPDAAPP_UpdatedBy { get; set; }
    }
}
