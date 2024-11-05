using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Yearly_RebateSetting")]
    public class FeeYearlyRebateSettingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYREBSET_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string FYREBSET_RebateTypeFlg { get; set; }
        public DateTime FYREBSET_RebateDate { get; set; }
        public Decimal FYREBSET_RebateAmtOrPercentValue { get; set; }
        public bool FYREBSET_ActiveId { get; set; }


    }
}
