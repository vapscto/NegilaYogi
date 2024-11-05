using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_PurchaseRequisition", Schema = "INV")]
    public class INV_M_PurchaseRequisitionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long? HRME_Id { get; set; }
        public string INVMPR_PRNo { get; set; }
        public DateTime INVMPR_PRDate { get; set; }
        public string INVMPR_Remarks { get; set; }
        public decimal INVMPR_ApproxTotAmount { get; set; }
        public bool INVMPR_PICreatedFlg { get; set; }
        public bool INVMPR_ActiveFlg { get; set; }
        public long INVMPR_CreatedBy { get; set; }
        public long INVMPR_UpdatedBy { get; set; }
        public string INVMPR_Flag { get; set; }
        public long? AMST_Id { get; set; }
        public List<INV_T_PurchaseRequisitionDMO> INV_T_PurchaseRequisitionDMO { get; set; }


    }
}
