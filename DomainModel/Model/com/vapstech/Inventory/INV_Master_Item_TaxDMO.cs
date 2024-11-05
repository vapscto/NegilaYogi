using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Item_Tax", Schema = "INV")]
    public class INV_Master_Item_TaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMIT_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        public bool INVMIT_ActiveFlg { get; set; }



    }
}
