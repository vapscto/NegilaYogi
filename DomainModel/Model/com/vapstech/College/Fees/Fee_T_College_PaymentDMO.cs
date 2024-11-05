using DomainModel.Model.com.vaps.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_T_College_Payment", Schema ="CLG")]
    public class Fee_T_College_PaymentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
        public long FTCP_Id { get; set; }
        public long FYP_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public decimal FTCP_PaidAmount { get; set; }
        public decimal FTCP_WaivedAmount { get; set; }
        public decimal FTCP_ConcessionAmount { get; set; }
        public decimal FTCP_FineAmount { get; set; }
        public decimal FTCP_RebateAmount { get; set; }
        public string FTCP_Remarks { get; set; }
        public DateTime? FTCP_UpdatedDate { get; set; }

        public DateTime? FTCP_CreatedDate { get; set; } 
  
    }
}
