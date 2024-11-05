using DomainModel.Model.com.vaps.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Y_Payment_College_Student", Schema ="CLG")]
    public class Fee_Y_Payment_College_StudentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPCS_Id { get; set; }
        public long FYP_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public decimal FYPCS_TotalPaidAmount { get; set; }
        public decimal FYPCS_TotalWaivedAmount { get; set; }
        public decimal FYPCS_TotalConcessionAmount { get; set; }
        public decimal FYPCS_TotalFineAmount { get; set; }
       
    }
}
