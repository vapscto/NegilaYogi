using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_PurchaseIndent", Schema = "INV")]
    public class INV_M_PurchaseIndentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPI_Id { get; set; }
        public long MI_Id { get; set; }     
        public string INVMPI_PINo { get; set; }
        public DateTime INVMPI_PIDate { get; set; }
        public string INVMPI_Remarks { get; set; }
        public string INVMPI_ReferenceNo { get; set; }
        public decimal INVMPI_ApproxTotAmount { get; set; }
        public bool INVMPI_POCreatedFlg { get; set; }
        public bool INVMPI_ActiveFlg { get; set; }
        public long INVMPI_CreatedBy { get; set; }
        public long INVMPI_UpdatedBy { get; set; }
       
        public List<INV_T_PurchaseIndentDMO> INV_T_PurchaseIndentDMO { get; set; }
     
    }
}
