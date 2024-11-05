using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_LeavingReason")]
    public class masterLeavingReasonDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMLREA_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMLREA_LeavingReason { get; set; }
        public bool HRMLREA_TransferredFlg { get; set; }
        public bool HRMLREA_ActiveFlg { get; set; }
        public long HRMLREA_CreatedBy { get; set; }
        public long HRMLREA_UpdatedBy { get; set; }
        public DateTime? HRMLREA_CreatedDate { get; set; }
        public DateTime? HRMLREA_UpdatedDate { get; set; }
    }
}
