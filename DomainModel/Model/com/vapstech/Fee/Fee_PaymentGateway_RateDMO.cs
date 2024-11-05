using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_PaymentGateway_Rate")]
    public class Fee_PaymentGateway_RateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FPGR_Id { get; set; }
        public long MI_Id { get; set; }
        public long IMPG_Id { get; set; }
        public decimal FPGR_Rate { get; set; }
        public DateTime? FPGR_CreatedDate { get; set; }
        public DateTime? FPGR_UpdatedDate { get; set; }
        public long? FPGR_CreatedBy { get; set; }
        public long? FPGR_UpdatedBy { get; set; }
        public string FPGR_CardType { get; set; }
        public string FPGR_CardNetworkType { get; set; }
         public decimal FPGR_UpToAmount { get; set; }
        public decimal FPGR_LessthanOrGreatherThan { get; set; }
        public decimal FPGR_InstitutionRate { get; set; }
        public decimal FPGR_VAPSRate { get; set; }


    }
}
