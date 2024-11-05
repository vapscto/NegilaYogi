using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Studentwise_Amount", Schema = "CLG")]
    public class Clg_Fee_Studentwise_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FCSA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public decimal FCSA_Amount { get; set; }
        public string FCSA_Currency { get; set; }
        public bool FCSA_ActiveFlg { get; set; }
      
    }
}
