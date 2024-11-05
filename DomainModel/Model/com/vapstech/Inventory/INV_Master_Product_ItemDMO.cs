using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Product_Item", Schema = "INV")]
    public class INV_Master_Product_ItemDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPI_Id { get; set; }
        public long INVMP_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal INVMPI_ItemQty { get; set; }
        public bool INVMPI_ActiveFlg { get; set; }

 


    }
}
