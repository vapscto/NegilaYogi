using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Student_Concession")]
    public class FeeConcessionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMC_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_ID { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public string FSC_ConcessionReason { get; set; }
        public string FSC_ConcessionType { get; set; }
        public string FMSG_ActiveFlag { get; set; }
        public long? FSC_CreatedBy { get; set; }
        public long? FSC_UpdatedBy { get; set; }

    }
}
