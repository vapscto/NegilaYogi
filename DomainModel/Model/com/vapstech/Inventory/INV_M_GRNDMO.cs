using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_GRN", Schema = "INV")]
    public class INV_M_GRNDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMGRN_Id { get; set; }
        public long MI_Id { get; set; }
        public long? INVMS_Id { get; set; }
        public string INVMGRN_GRNNo { get; set; }
        public string INVMGRN_InvoiceNo { get; set; }
        public DateTime? INVMGRN_PurchaseDate { get; set; }
        public decimal INVMGRN_PurchaseValue { get; set; }
        public decimal? INVMGRN_TotDiscount { get; set; }
        public decimal? INVMGRN_TotTaxAmt { get; set; }
        public decimal INVMGRN_TotalAmount { get; set; }
        public string INVMGRN_Remarks { get; set; }
        public string INVMGRN_ReturnFlg { get; set; }
        public string INVMGRN_PaidFlg { get; set; }
        public bool INVMGRN_CreditFlg { get; set; }
        public bool INVMGRN_ActiveFlg { get; set; }
        public decimal INVMGRN_TotalPaid { get; set; }
        public decimal INVMGRN_TotalBalance { get; set; }

        public List<INV_T_GRNDMO> INV_T_GRNDMO { get; set; }
        public List<INV_M_GRN_StoreDMO> INV_M_GRN_StoreDMO { get; set; }
      //  public List<INV_M_GRN_PODMO> INV_M_GRN_PODMO { get; set; }





    }
}
