using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.PDA
{
    [Table("PDA_Refund")]
    public class PDA_RefundDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long PDAR_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long PDAMH_Id { get; set; }
        public DateTime PDAR_Date { get; set; }
        public string PDAR_RefundNo { get; set; }
        public decimal PDAR_RefundAmount { get; set; }
        public string PDAR_ModeOfPayment { get; set; }
        public string PDAR_ChequeDDNo { get; set; }
        public string PDAR_RefundRemarks { get; set; }
        public DateTime PDAR_ChequeDDDate { get; set; }
        public string PDAR_BankName { get; set; }
        public string PDAR_OPReferenceNo { get; set; }
        public bool PDAR_ActiveFlag { get; set; }
     
        
    }
}
