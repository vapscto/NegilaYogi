using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Adm_TC_Library_Approval")]
    public class Adm_TC_Library_Approval_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ATCLIBAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime ATCLIBAPP_ApprovedDate { get; set; }
        public long ATCLIBAPP_ApprovedBy { get; set; }
        public bool ATCLIBAPP_ApprovalFlg { get; set; }
        public string ATCLIBAPP_Remarks { get; set; }
        public bool ATCLIBAPP_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ATCLIBAPP_CreatedBy { get; set; }
        public long ATCLIBAPP_UpdatedBy { get; set; }
    }
}
