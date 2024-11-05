using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Adm_TC_Fee_Approval")]
    public class Adm_TC_Fee_Approval_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ATCFAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime ATCFAPP_ApprovedDate { get; set; }
        public long ATCFAPP_ApprovedBy { get; set; }
        public long ATCFAPP_FeeGroupId { get; set; }
        public bool ATCFAPP_ApprovalFlg { get; set; }
        public string ATCFAPP_Remarks { get; set; }
        public bool ATCFAPP_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ATCFAPP_CreatedBy { get; set; }
        public long ATCFAPP_UpdatedBy { get; set; }
    }
}
