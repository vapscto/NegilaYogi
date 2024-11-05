using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_T_Payment_OthStaff_College", Schema = "CLG")]
    public class Fee_T_Payment_OthStaff_College_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FTPOSTC_Id { get; set; }
        [ForeignKey("FYP_Id")]
        public long FYP_Id { get; set; }
        public long FMCAOST_Id  { get; set; }
        public long FTPOSTC_PaidAmount { get; set; }
        public long FTPOSTC_WaivedAmount { get; set; }
        public long FTPOSTC_ConcessionAmount { get; set; }
        public decimal FTPOSTC_FineAmount { get; set; }
        public decimal FTPOSTC_RebateAmount { get; set; }
        public string FTPOSTC_Remarks { get; set; }


       public DateTime? FTPOSTC_CreatedDate { get; set; }
        public DateTime? FTPOSTC_UpdatedDate { get; set; }
        public long? FTPOSTC_CreatedBy    { get; set; }
    public long? FTPOSTC_UpdatedBy { get; set; }

    }
}
