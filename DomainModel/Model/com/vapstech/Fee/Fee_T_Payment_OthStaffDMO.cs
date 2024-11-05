using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_T_Payment_OthStaff")]
    public class Fee_T_Payment_OthStaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FTPOST_Id { get; set; }
        [ForeignKey("FYP_Id")]
        public long FYP_Id { get; set; }
        public long FMAOST_Id { get; set; }
      //  public long ASMAY_Id { get; set; }
        public long FTPOST_PaidAmount { get; set; }
        public long FTPOST_WaivedAmount { get; set; }
        public long FTPOST_ConcessionAmount { get; set; }
        public decimal FTPOST_FineAmount { get; set; }
        public decimal FTPOST_RebateAmount { get; set; }
        public string FTPOST_Remarks { get; set; }

    }
}
