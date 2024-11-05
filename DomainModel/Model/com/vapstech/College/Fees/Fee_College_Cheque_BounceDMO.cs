using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Cheque_Bounce", Schema = "CLG")]
    public class Fee_College_Cheque_BounceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCCB_Id { get; set; }
        public long MI_Id { get; set; }
        public long FYP_Id { get; set; }
        public DateTime FCCB_Date { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public decimal FCCB_Amount { get; set; }
        public string FCCB_Remarks { get; set; }
        public long User_Id { get; set; }
    }
}
