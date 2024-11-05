using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Others_Concession_College", Schema = "CLG")]
    public class Fee_Others_Concession_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCOST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FCMA_Id { get; set; }
        public long FMG_Id { get; set; }

        public long FMH_Id { get; set; }
        public string FOCC_ConcessionReason { get; set; }
        public string FOCC_ConcessionType { get; set; }

        public bool FOCC_ActiveFlag { get; set; }
        public DateTime FOCC_CreatedDate { get; set; }
        public DateTime FOCC_UpdatedDate { get; set; }
        public long FOCC_CreatedBy { get; set; }
        public long FOCC_UpdatedBy { get; set; }
    }
}
