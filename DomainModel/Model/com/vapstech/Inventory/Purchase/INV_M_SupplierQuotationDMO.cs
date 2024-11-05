using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_SupplierQuotation", Schema = "INV")]
    public class INV_M_SupplierQuotationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public string INVMSQ_QuotationNo { get; set; }
        public string INVMSQ_SupplierName { get; set; }
        public long INVMSQ_SupplierContactNo { get; set; }
        public string INVMSQ_SupplierEmailId { get; set; }
        public string INVMSQ_Quotation { get; set; }
        public decimal INVMSQ_TotalQuotedRate { get; set; }
        public decimal INVMSQ_NegotiatedRate { get; set; }
        public string INVMSQ_Remarks { get; set; }
        public bool INVMSQ_FinaliseFlg { get; set; }
        public bool INVMSQ_ActiveFlg { get; set; }
        public long INVMSQ_CreatedBy { get; set; }
        public long INVMSQ_UpdatedBy { get; set; }
        public List<INV_T_SupplierQuotationDMO> INV_T_SupplierQuotationDMO { get; set; }

    }
}
