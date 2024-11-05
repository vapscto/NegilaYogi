using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_PurchaseOrder", Schema = "INV")]
    public class INV_M_PurchaseOrderDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPO_Id { get; set; }
        public long MI_Id { get; set; }
        public long? INVMS_Id { get; set; }
        public string INVMPO_PONo { get; set; }
        public long INVMSQ_Id { get; set; }
        public DateTime INVMPO_PODate { get; set; }
        public string INVMPO_Remarks { get; set; }
        public string INVMPO_ReferenceNo { get; set; }
        public decimal INVMPO_TotRate { get; set; }
        public decimal INVMPO_TotTax { get; set; }
        public decimal INVMPO_TotAmount { get; set; }
        public bool INVMPO_ActiveFlg { get; set; }
        public long INVMPO_CreatedBy { get; set; }
        public long INVMPO_UpdatedBy { get; set; }
        public string INVMPO_POTemplate { get; set; }
        public List<INV_T_PurchaseOrderDMO> INV_T_PurchaseOrderDMO { get; set; }

    }
}
