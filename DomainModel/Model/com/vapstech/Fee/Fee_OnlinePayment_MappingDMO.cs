using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_OnlinePayment_Mapping")]
    public class Fee_OnlinePayment_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FOPM_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMH_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long fpgd_id { get; set; }
        public long fmg_id { get; set; }
        public long fti_id { get; set; }
        public long fmt_id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool PreAdmFlag { get; set; }

        public long? FOPM_CreatedBy { get; set; }
        public long? FOPM_UpdatedBy { get; set; }
    }
}
