using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Master_Amount",Schema ="CLG")]
    public class Clg_Fee_AmountEntry_DMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FCMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FTI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public string FCMA_Flag { get; set; }
        public long FMH_Id { get; set; }
        public bool FCMA_ActiveFlg { get; set; }
    }
}
