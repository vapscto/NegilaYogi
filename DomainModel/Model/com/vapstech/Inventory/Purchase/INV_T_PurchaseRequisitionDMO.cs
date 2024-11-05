using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_T_PurchaseRequisition", Schema = "INV")]
    public class INV_T_PurchaseRequisitionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTPR_Id { get; set; }
        public long INVMPR_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public decimal INVTPR_PRQty { get; set; }
        public decimal INVTPR_PRUnitRate { get; set; }        
        public decimal INVTPR_ApproxAmount { get; set; }
        public decimal INVTPR_ApprovedQty { get; set; }
        public string INVTPR_Remarks { get; set; }
        public bool INVTPR_ActiveFlg { get; set; }     
        public long INVTPR_CreatedBy { get; set; }  
        public long INVTPR_UpdatedBy { get; set; }
      //  public INV_M_PurchaseRequisitionDMO INV_M_PurchaseRequisitionDMO { get; set; }


    }
}
