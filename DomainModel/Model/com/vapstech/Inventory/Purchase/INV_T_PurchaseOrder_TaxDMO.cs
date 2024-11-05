using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_T_PurchaseOrder_Tax", Schema = "INV")]
    public class INV_T_PurchaseOrder_TaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTPOT_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVTPO_Id { get; set; }
        public long INVMIT_Id { get; set; }
        public decimal INVTPOT_TaxPercent { get; set; }
        public decimal INVTPOT_TaxAmount { get; set; }
        public bool INVTPOT_ActiveFlg { get; set; }
        public long INVTPOT_CreatedBy { get; set; }
        public long INVTPOT_UpdatedBy { get; set; }

        public INV_T_PurchaseOrderDMO INV_T_PurchaseOrderDMO { get; set; }
     
    }
}
