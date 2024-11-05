using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_T_PurchaseIndent", Schema = "INV")]
    public class INV_T_PurchaseIndentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTPI_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long? INVMPR_Id { get; set; }
        public decimal INVTPI_PRQty { get; set; }
        public decimal INVTPI_PIUnitRate { get; set; }
        public decimal INVTPI_ApproxAmount { get; set; }
        public decimal INVTPI_PIQty { get; set; }
        public string INVTPI_Remarks { get; set; }
        public bool INVTPI_ActiveFlg { get; set; }
        public long INVTPI_CreatedBy { get; set; }
        public long INVTPI_UpdatedBy { get; set; }

        public INV_M_PurchaseIndentDMO INV_M_PurchaseIndentDMO { get; set; }


    }
}
