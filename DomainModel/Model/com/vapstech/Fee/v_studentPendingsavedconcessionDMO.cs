using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("v_studentPendingsavedconcession")]
    public class v_studentPendingsavedconcessionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VSPSC_ID { get; set; }
        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }

        public long FTI_Id { get; set; }
        public long fmh_id { get; set; }
        public long FMA_Amount { get; set; }
        public long FMG_Id { get; set; }
        public long FSCI_ConcessionAmount { get; set; }

        public string FSC_ConcessionType { get; set; }
        public string FSC_ConcessionReason { get; set; }

        public long mi_id { get; set; }
        public long fma_id { get; set; }
        public long asmay_id { get; set; }

        public long fsc_id { get; set; }
    }
}
