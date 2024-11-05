using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Studentwise_PDC", Schema = "CLG")]
    //[Table("Fee_College_Studentwise_PDC")]
    public class Fee_College_Studentwise_PDCDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSPDC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string FCSPDC_ChequeNo { get; set; }
        public DateTime? FCSPDC_ChequeDate { get; set; }
        public decimal FCSPDC_Amount { get; set; }
        public long FMBANK_Id { get; set; }
        public string FCSPDC_Currency { get; set; }
        public string FCSPDC_Narration { get; set; }
        public string FCSPDC_Status { get; set; }
        public bool FCSPDC_ActiveFlg { get; set; }
        public DateTime FCSPDC_CreatedDate { get; set; }
        public DateTime FCSPDC_UpdatedDate { get; set; }
        public long FCSPDC_CreatedBy { get; set; }
        public long FCSPDC_Updatedby { get; set; }

    }
}
