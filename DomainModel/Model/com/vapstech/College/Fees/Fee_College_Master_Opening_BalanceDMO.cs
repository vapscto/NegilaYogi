using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Master_Opening_Balance", Schema = "CLG")]
    public class Fee_College_Master_Opening_BalanceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCMOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public DateTime FCMOB_EntryDate { get; set; }
        public decimal FCMOB_Student_Due { get; set; }
        public decimal FCMOB_Institution_Due { get; set; }
        public decimal FCMOB_RefundandableAmount { get; set; }
        public bool FCMOB_ActiveFlg { get; set; }        
        public long User_Id { get; set; }
    }
}
