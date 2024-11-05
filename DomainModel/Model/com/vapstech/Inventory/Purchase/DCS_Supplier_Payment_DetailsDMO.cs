using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("DCS_Supplier_Payment_Details", Schema = "DCS")]
    public class DCS_Supplier_Payment_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DCSSPTD_Id { get; set; }
        public long DCSSPT_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal INVSPTGRN_Amount { get; set; }     
        public string INVSPTGRN_Remarks { get; set; }
        public bool INVSPTGRN_ActiveFlg { get; set; }
        public long INVSPTGRN_CreatedBy { get; set; }     
        public long INVSPTGRN_UpdatedBy { get; set; }

        public DCS_Supplier_PaymentDMO DCS_Supplier_Payment { get; set; }
    }
}
