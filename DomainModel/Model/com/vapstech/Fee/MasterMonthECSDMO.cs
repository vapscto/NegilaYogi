using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("adm_master_month_ecs")]
    public class MasterMonthECSDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMME_ID { get; set; }
        public string AMM_NAME { get; set; }
        public long FTDDE_Month { get; set; }
        public int AMME_Month_Max_Days { get; set; }
    }
}
