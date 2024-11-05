using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_PDC")]
    public class FeePDCDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FPDC_Id {get;set;}
    public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMT_Id { get; set; }
        public string  FPDC_ChequeNo { get; set; }
        public DateTime FPDC_ChequeDate { get; set; }
        public decimal FCSPDC_Amount { get; set; }
        public long FMBANK_Id { get; set; }
        public string FPDC_Currency { get; set; }
        public string FPDC_Narration { get; set; }
        public string  FPDC_Status { get; set; }
        public bool FPDC_ActiveFlg { get; set; }
        public DateTime? FPDC_CreatedDate { get; set; }
        public DateTime? FPDC_UpdatedDate { get; set; }
        public long? FPDC_CreatedBy { get; set; }
        public long? FPDC_Updatedby {get;set;}
}
}
