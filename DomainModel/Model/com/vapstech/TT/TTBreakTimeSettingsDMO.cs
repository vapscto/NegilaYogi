using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Break")]
    public class TTBreakTimeSettingsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long TTMD_Id { get; set; }
        public long TTMC_Id { get; set; }
        public decimal TTMB_AfterPeriod { get; set; }
        public string TTMB_BreakName { get; set; }
        public string TTMB_BreakStartTime { get; set; }
        public string TTMB_BreakEndTime { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool TTMB_ActiveFlag { get; set; }
        

    }
}
