using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Student_Concession", Schema = "CLG")]
    public class CollegeConcessionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSC_Id { get; set; }

        public long MI_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FCMAS_Id { get; set; }

        public long FMG_Id { get; set; }

        public long FMH_Id { get; set; }

        public string FCSC_ConcessionReason { get; set; }

        public string FCSC_ConcessionType { get; set; }

        public bool FCSC_ActiveFlag { get; set; }
    }
}
