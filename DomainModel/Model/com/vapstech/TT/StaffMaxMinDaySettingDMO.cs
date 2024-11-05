using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Staffwise_MaxMinDays")]
    public class StaffMaxMinDaySettingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTPMMD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public int TTMP_Id { get; set; }
        public long TTPMMD_MaxDay { get; set; }
        public long TTPMMD_MinDay { get; set; }
        public long TTMC_Id { get; set; }
        public bool TTPMMD_ActiveFlag { get; set; }

    }
}
