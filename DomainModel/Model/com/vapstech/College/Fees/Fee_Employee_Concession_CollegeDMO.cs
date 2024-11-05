using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Employee_Concession_College", Schema = "CLG")]
    public class Fee_Employee_Concession_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FECC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long HRME_Id { get; set; }

        public long ASMAY_Id { get; set; }
        public long FCMA_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }

        public string FECC_ConcessionReason { get; set; }
        public string FECC_ConcessionType { get; set; }
        public bool FECC_ActiveFlag { get; set; }


        public DateTime FECC_CreatedDate { get; set; }

        public DateTime FECC_UpdatedDate { get; set; }

        public long FECC_CreatedBy { get; set; }

        public long FECC_UpdatedBy { get; set; }


    }
}
