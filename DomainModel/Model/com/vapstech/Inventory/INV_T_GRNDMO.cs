using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_GRN", Schema = "INV")]
    public class INV_T_GRNDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTGRN_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVTGRN_BatchNo { get; set; }
        public decimal INVTGRN_PurchaseRate { get; set; }
        public decimal INVTGRN_MRP { get; set; }
        public decimal INVTGRN_SalesPrice { get; set; }
        public decimal INVTGRN_DiscountAmt { get; set; }
        public decimal INVTGRN_TaxAmt { get; set; }
        public decimal INVTGRN_Amount { get; set; }
        public decimal INVTGRN_Qty { get; set; }
        public string INVTGRN_Naration { get; set; }
        public DateTime? INVTGRN_MfgDate { get; set; }
        public DateTime? INVTGRN_ExpDate { get; set; }
        public bool INVTGRN_ReturnFlg { get; set; }
        public decimal INVTGRN_ReturnQty { get; set; }
        public DateTime? INVTGRN_ReturnDate { get; set; }
        public string INVTGRN_ReturnNo { get; set; }
        public string INVTGRN_ReturnNaration { get; set; }
        public bool INVTGRN_ActiveFlg { get; set; }
        public INV_M_GRNDMO INV_M_GRNDMO { get; set; }

       public List<INV_T_GRN_TaxDMO> INV_T_GRN_TaxDMO { get; set; }
        



    }
}
