using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Consecutive")]
    public class TT_ConsecutiveDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal TTC_NoOfPeriods { get; set; }
        public decimal TTC_AllotPeriods { get; set; }
        public decimal TTC_RemPeriods { get; set; }
        public decimal TTC_NoOfConPeriods { get; set; }
        public decimal TTC_NoOfConDays { get; set; }
        public int TTC_BefAftApplFlag { get; set; }
        public int TTC_BefAftFalg { get; set; }
        public decimal TTC_BefAftPeriod { get; set; }
        public string TTC_AllotedFlag { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool TTC_ActiveFlag { get; set; }

    }
}
