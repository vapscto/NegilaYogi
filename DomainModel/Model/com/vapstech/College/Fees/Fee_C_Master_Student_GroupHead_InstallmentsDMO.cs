using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_C_Master_Student_GroupHead_Installments",Schema ="CLG")]
    public class Fee_C_Master_Student_GroupHead_InstallmentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCMSGHI_Id { get; set; }
        public long FCMSGH_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
       
    }
}
