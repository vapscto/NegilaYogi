using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_Group")]
    public class FeeGroupClgDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public string FMG_Remarks { get; set; }
        public string FMG_CompulsoryFlag { get; set; }
        public bool FMG_ActiceFlag { get; set; }
        public long user_id { get; set; }
        public int? FMG_Order { get; set; }
        public bool? FMG_BatchwiseFeeApplFlg { get; set; }
        public bool? FMG_TransportFlg { get; set; }
        public bool? FMG_HostelFlg { get; set; }
        public string FMG_RegNewFlg { get; set; }

    }
}
