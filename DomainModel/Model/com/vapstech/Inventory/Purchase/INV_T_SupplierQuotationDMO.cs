using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_T_SupplierQuotation", Schema = "INV")]
    public class INV_T_SupplierQuotationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTSQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMSQ_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public decimal INVTSQ_QuotedRate { get; set; }
        public decimal INVTSQ_NegotiatedRate { get; set; }
        public bool INVTSQ_FinaliseFlg { get; set; }
        public bool INVTSQ_ActiveFlg { get; set; }
        public long INVTSQ_CreatedBy { get; set; }
        public long INVTSQ_UpdatedBy { get; set; }

        public INV_M_SupplierQuotationDMO INV_M_SupplierQuotationDMO { get; set; }


    }
}
