using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Refund", Schema = "CLG")]
    public class Fee_College_RefundDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCR_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public DateTime? FCR_Date { get; set; }
        public string FCR_RefundNo { get; set; }
        public decimal FCR_RefundAmount { get; set; }
        public string FCR_ModeOfPayment { get; set; }
        public long FCR_ChequeDDNo { get; set; }
        public string FCR_RefundRemarks { get; set; }
        public DateTime? FCR_ChequeDDDate { get; set; }
        public string FCR_Bank { get; set; }
        public long FCR_OPReferenceNo { get; set; }
        public string FCR_RefundFlag { get; set; }
        public long User_Id { get; set; }
    }
}
