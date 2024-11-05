using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_Sales", Schema = "INV")]
    public class INV_T_SalesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTSL_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        //  public long INVSTO_Id { get; set; }
        public string INVTSL_BatchNo { get; set; }
        public decimal? INVTSL_SalesQty { get; set; }
        public decimal? INVTSL_SalesPrice { get; set; }
        public decimal? INVTSL_DiscountAmt { get; set; }
        public decimal? INVTSL_TaxAmt { get; set; }
        public decimal? INVTSL_Amount { get; set; }
        public string INVTSL_Naration { get; set; }
        public bool? INVTSL_ReturnFlg { get; set; }
        public decimal? INVTSL_ReturnQty { get; set; }
        public DateTime? INVTSL_ReturnDate { get; set; }
        public string INVTSL_ReturnNo { get; set; }
        public string INVTSL_ReturnNaration { get; set; }
        public bool? INVTSL_ActiveFlg { get; set; }
        public INV_M_SalesDMO INV_M_SalesDMO { get; set; }
        public List<INV_T_Sales_TaxDMO> INV_T_Sales_TaxDMO { get; set; }


    }
}
