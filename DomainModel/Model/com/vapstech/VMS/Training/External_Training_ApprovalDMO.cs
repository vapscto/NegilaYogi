using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_External_Training_Approval")]
    public class External_Training_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREXTTRNAPP_Id { get; set; }
        public long HREXTTRN_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREXTTRNAPP_ApproverRemarks { get; set; }
        public decimal? HREXTTRNAPP_ApprovedHours { get; set; }
        public decimal? HREXTTRNAPP_ApprovedHrs { get; set; }
        public bool HREXTTRNAPP_ActiveFlag { get; set; }
        public DateTime HREXTTRNAPP_CreatedDate { get; set; }
        public DateTime HREXTTRNAPP_UpdatedDate { get; set; }
        public long HREXTTRNAPP_CreatedBy { get; set; }
        public long HREXTTRNAPP_UpdatedBy { get; set; }
        public string HREXTTRNAPP_ApprovalFlg { get; set; }
    }
}
