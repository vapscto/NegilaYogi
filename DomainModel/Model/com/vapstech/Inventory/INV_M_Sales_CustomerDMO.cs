using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_Sales_Customer", Schema = "INV")]
    public class INV_M_Sales_CustomerDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSLC_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long INVMC_Id { get; set; }
        public bool? INVMSLC_ActiveFlg { get; set; }
        public INV_M_SalesDMO INV_M_SalesDMO { get; set; }
    }
}
