using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Master_Student_GroupHead",Schema ="CLG")]

    public class Fee_College_Master_Student_GroupHeadDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCMSGH_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set;}
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool FCMSGH_ActiveFlag { get; set; }
        public List<Fee_C_Master_Student_GroupHead_InstallmentsDMO> Fee_C_Master_Student_GroupHead_InstallmentsDMO { get; set; }
    }
}
