using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Product", Schema = "INV")]
    public class INV_Master_ProductDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMP_ProductName { get; set; }
        public string INVMP_ProductCode { get; set; }
        public decimal INVMP_ProductPrice { get; set; }
        public bool INVMP_TaxApplFlg { get; set; }
        public bool INVMP_ActiveFlg { get; set; }

        public List<INV_Master_Product_TaxDMO> INV_Master_Product_TaxDMO { get; set; }        





    }
}
