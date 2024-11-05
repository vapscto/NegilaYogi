using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Student_Adjustment", Schema = "CLG")]
    public class Fee_College_Student_AdjustmentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FCSA_Date { get; set; }
        public long FCSA_From_FMH_Id { get; set; }
        public long FCSA_From_FMG_Id { get; set; }
        public long FCSA_FromFMA_Id { get; set; }
        public long FCSA_FromFTI_Id { get; set; }
        public long FCSA_AdjustedAmount { get; set; }
        public long FCSA_To_FMH_Id { get; set; }
        public long FCSA_To_FMG_Id { get; set; }
        public long FCSA_ToFMA_Id { get; set; }
        public long FCSA_ToFTI_Id { get; set; }
        public bool FCSA_ActiveFlag { get; set; }
        public long User_Id { get; set; }
    }
}
