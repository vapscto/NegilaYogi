using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Adm_TC_CT_Approval")]
    public class Adm_TC_CT_Approval_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ATCCTAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime ATCCTAPP_ApprovedDate { get; set; }
        public long ATCCTAPP_ApprovedBy { get; set; }
        public bool ATCCTAPP_AttendanceApprovalFlg { get; set; }
        public bool ATCCTAPP_ExamApprovalFlg { get; set; }
        public string ATCCTAPP_Remarks { get; set; }
        public bool ATCCTAPP_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ATCCTAPP_CreatedBy { get; set; }
        public long ATCCTAPP_UpdatedBy { get; set; }
    }
}
