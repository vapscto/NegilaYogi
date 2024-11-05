using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_Sales_College_Student", Schema = "INV")]
    public class INV_M_Sales_College_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSLCS_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool INVMSLCS_ActiveFlg { get; set; }

        public INV_M_SalesDMO INV_M_SalesDMO { get; set; }
    }
}
