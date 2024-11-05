using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Y_Payment_College_Staff", Schema = "CLG")]
    public class Fee_Y_Payment_College_StaffDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPCS_Id { get; set; }
        [ForeignKey("FYP_Id")]
        public long FYP_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FYPCS_TotalPaidAmount { get; set; }
        public long FYPCS_TotalWaivedAmount { get; set; }
        public long FYPCS_TotalConcessionAmount { get; set; }
        public decimal FYPCS_TotalFineAmount { get; set; }

        public DateTime?     FYPCS_CreatedDate { get; set; }
        public DateTime?  FYPCS_UpdatedDate { get; set; }
        public long? FYPCS_CreatedBy { get; set; }
        public long? FYPCS_UpdatedBy { get; set; }

    }
}
