using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Y_Payment_OthStu_College", Schema = "CLG") ]
    public class Fee_Y_Payment_OthStu_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPOSTC_Id { get; set; }
        [ForeignKey("FYP_Id")]
        public long FYP_Id { get; set; }
        public long FMCOST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FYPOSTC_TotalPaidAmount { get; set; }
        public long FYPOSTC_TotalWaivedAmount { get; set; }
        public long FYPOSTC_TotalConcessionAmount { get; set; }
        public decimal FYPOSTC_TotalFineAmount { get; set; }

     
         public DateTime   FYPOSTC_CreatedDate { get; set; }
        public DateTime FYPOSTC_UpdatedDate{ get; set; }
        public long FYPOSTC_CreatedBy { get; set; }
        public long  FYPOSTC_UpdatedBy { get; set; }
    }
}
