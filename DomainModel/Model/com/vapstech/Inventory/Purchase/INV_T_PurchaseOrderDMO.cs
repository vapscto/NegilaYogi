using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_T_PurchaseOrder", Schema = "INV")]
    public class INV_T_PurchaseOrderDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTPO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPO_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public decimal INVTPO_POQty { get; set; }
        public decimal INVTPO_RatePerUnit { get; set; }
        public decimal INVTPO_TaxAmount { get; set; }
        public decimal INVTPO_Amount { get; set; }
        public string INVTPO_Remarks { get; set; }
        public bool INVTPO_ActiveFlg { get; set; }
        public long INVTPO_CreatedBy { get; set; }
        public long INVTPO_UpdatedBy { get; set; }
        public DateTime? INVTPO_ExpectedDeliveryDate { get; set; }
        public INV_M_PurchaseOrderDMO INV_M_PurchaseOrderDMO { get; set; }
        public List<INV_T_PurchaseOrder_TaxDMO> INV_T_PurchaseOrder_TaxDMO { get; set; }
     
    }
}
