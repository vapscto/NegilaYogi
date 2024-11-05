using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Proforma_Invoice")]
    public class ISM_Proforma_InvoiceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMPRINC_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public string ISMPRINC_WorkOrder { get; set; }
        public string ISMPRINC_PrInviceNo { get; set; }
        public DateTime ISMPRINC_Date { get; set; }
        public decimal ISMPRINC_TotalTaxAmount { get; set; }
        public decimal ISMPRINC_TotalAmount { get; set; }
        public string ISMPRINC_Remarks { get; set; }
        public bool ISMPRINC_ActiveFlag { get; set; }
        public DateTime ISMPRINC_CreatedDate { get; set; }
        public long ISMPRINC_CreatedBy { get; set; }
        public DateTime ISMPRINC_UpdatedDate { get; set; }
        public long ISMPRINC_UpdatedBy { get; set; }
        public decimal ISMPRINC_AdvPer { get; set; }
        public decimal ISMPRINC_AdvanceAmount { get; set; }
        public string ISMPRINC_ModeOfPayment { get; set; }
        public string ISMPRINC_MOURefNo { get; set; }
        public DateTime? ISMPRINC_MOUDate { get; set; }
        public long? HRMBD_Id { get; set; }

        public List<ISM_Proforma_Invoice_DetailsDMO> ISM_Proforma_Invoice_DetailsDMO { get; set; }
        public List<ISM_Proforma_Invoice_TaxDMO> ISM_Proforma_Invoice_TaxDMO { get; set; }
        //public string ISMPRINC_InstallmentName { get; set; }
    }
}
