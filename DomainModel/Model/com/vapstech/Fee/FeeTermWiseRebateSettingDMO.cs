using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_Terms_RebateSetting")]
    public class FeeTermWiseRebateSettingDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMTRS_Id { get; set; }
        public long FMT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FMTRS_RebateApplicableDate { get; set; }
        public string FMTRS_RebateAmountPercentFlg { get; set; }
        public decimal FMTRS_RebateAmountPercentValue { get; set; }

    }
}
