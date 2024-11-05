using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_Sales_Tax", Schema = "INV")]
    public class INV_T_Sales_TaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTSLT_Id { get; set; }
        public long INVTSL_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal? INVTSLT_TaxPer { get; set; }
        public decimal INVTSLT_TaxAmt { get; set; }
        public bool? INVTSLT_ActiveFlg { get; set; }

        public INV_T_SalesDMO INV_T_SalesDMO { get; set; }


    }
}
