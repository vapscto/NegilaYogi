using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_College_Amount_OthStaffs", Schema = "CLG")]
    public class Fee_Master_College_Amount_OthStaffs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMCAOST_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public decimal FMCAOST_Amount { get; set; }
        public string FMCAOST_OthStaffFlag { get; set; }
        public bool FMCAOST_ActiveFlag { get; set; }

        public DateTime? FMCAOST_CreatedDate{ get; set; }
        public DateTime? FMCAOST_UpdatedDate { get; set; }
        public long? FMCAOST_CreatedBy { get; set; }
        public long?  FMCAOST_UpdatedBy { get; set; }

    }
}
