using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("adm_master_month")]
    public class MasterMonthDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMM_ID { get; set; }
        public string AMM_NAME { get; set; }
        public long FTDD_Month { get; set; }
        public int AMM_Month_Max_Days { get; set; }

    }
}
