using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_OtherIncome")]
    public  class HR_Master_OtherIncome
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMOI_Id

        { get; set; }
        public long MI_Id
        { get; set; }
        public string HRMOI_OtherIncomeName

        { get; set; }
        public bool HRMOI_OtherIncomeFlg

        { get; set; }
        public bool HRMOI_MaxLimitAplFlg

        { get; set; }
        public decimal? HRMOI_MaxLimit

        { get; set; }
        public bool HRMOI_ActiveFlg

        { get; set; }
        // public bool HRETDS_ActiveFlg { get; set; }
        public long HRMOI_CreatedBy
        { get; set; }
        public long HRMOI_UpdatedBy
        { get; set; }


    }
}
