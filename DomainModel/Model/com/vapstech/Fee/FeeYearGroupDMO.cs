using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Yearly_Group")]
    public class FeeYearGroupDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FYG_Id { get; set; }
        public long FMG_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool FYG_ActiveFlag { get; set; }
        public long user_id { get;set;}

        public long? FYG_CreatedBy { get; set; }
        public long? FYG_UpdatedBy { get; set; }

        public string FYG_RebateTypeFlg { get; set; }
        public bool? FYG_RebateApplicableFlg { get; set; }
        public Decimal? FYG_PartialRebateAmtOrPercentageValue { get; set; }



    }
}
