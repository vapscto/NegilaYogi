using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Employee_Concession")]
    public class Fee_Employee_ConcessionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FEC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long HRME_Id { get; set; }

        public long ASMAY_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }

        public string FEC_ConcessionReason { get; set; }
        public string FEC_ConcessionType { get; set; }
        public bool FEC_ActiveFlag { get; set; }
    }
}
