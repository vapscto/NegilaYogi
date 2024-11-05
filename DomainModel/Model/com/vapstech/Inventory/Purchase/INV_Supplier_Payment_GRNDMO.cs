using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_Supplier_Payment_GRN", Schema = "INV")]
    public class INV_Supplier_Payment_GRNDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVSPTGRN_Id { get; set; }
        public long INVSPT_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public decimal INVSPTGRN_Amount { get; set; }     
        public string INVSPTGRN_Remarks { get; set; }
        public bool INVSPTGRN_ActiveFlg { get; set; }
        public long INVSPTGRN_CreatedBy { get; set; }     
        public long INVSPTGRN_UpdatedBy { get; set; }

        public INV_Supplier_PaymentDMO INV_Supplier_Payment { get; set; }
    }
}
