using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("DCS_Supplier_Payment", Schema = "DCS")]
    public class DCS_Supplier_PaymentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DCSSPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMS_Id { get; set; }
        public DateTime INVSPT_PaymentDate { get; set; }
        public string INVSPT_ModeOfPayment { get; set; }
        public string INVSPT_PaymentReference { get; set; }
        public string INVSPT_ChequeDDNo { get; set; }
        public string INVSPT_BankName { get; set; }
        public DateTime? INVSPT_ChequeDDDate { get; set; }
        public decimal INVSPT_Amount { get; set; }
        public string INVSPT_Remarks { get; set; }
        public bool INVSPT_ActiveFlg { get; set; }
        public long INVSPT_CreatedBy { get; set; }     
        public long INVSPT_UpdatedBy { get; set; }
        public List<DCS_Supplier_Payment_DetailsDMO> DCS_Supplier_Payment_DetailsDMO { get; set; }

    }
}
