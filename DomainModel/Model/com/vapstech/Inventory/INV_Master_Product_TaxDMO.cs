using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Product_Tax", Schema = "INV")]
    public class INV_Master_Product_TaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPT_Id { get; set; }
        public long INVMP_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVMPT_TaxValue { get; set; }
        public bool INVMPT_ActiveFlg { get; set; }       
    }
}
