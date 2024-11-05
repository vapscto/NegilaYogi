using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Quota_FeeGroup", Schema = "CLG")]
    public class ClgQuotaFeeGroupDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCQCFG_Id { get; set; }



        public long MI_Id { get; set; }


        public long ACQC_Id { get; set; }

        public long FMG_Id { get; set; }

        public bool FCQCFG_CompulsoryFlg { get; set; }


        public bool FCQCFG_ActiveFlg { get; set; }



        public DateTime FCQCFG_CreatedDate { get; set; }


        public DateTime FCQCFG_UpdatedDate { get; set; }


        public long FCQCFG_CreatedBy { get; set; }


        public long FCQCFG_UpdatedBy { get; set; }
    }
}
