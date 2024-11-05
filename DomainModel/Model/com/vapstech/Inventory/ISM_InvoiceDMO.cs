using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Invoice")]
    public class ISM_InvoiceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMINC_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public string ISMINC_WorkOrder { get; set; }
        public string ISMINC_PrInviceNo { get; set; }
        public DateTime ISMINC_Date { get; set; }
        public decimal ISMINC_TotalTaxAmount { get; set; }
        public decimal ISMINC_TotalAmount { get; set; }
        public string ISMINC_Remarks { get; set; }
        public bool ISMINC_ActiveFlag { get; set; }
        public DateTime ISMINC_CreatedDate { get; set; }
        public long ISMINC_CreatedBy { get; set; }
        public DateTime ISMINC_UpdatedDate { get; set; }
        public long ISMINC_UpdatedBy { get; set; }
        public long MI_Id { get; set; }
        public long? HRMBD_Id { get; set; }
        public DateTime? ISMINC_MOUDate { get; set; }
        public string ISMINC_MOURefNo { get; set; }
        public string ISMINC_ModeOfPayment { get; set; }
        //public string ISMINC_InstallmentName { get; set; }
        //public decimal? ISMINC_TotalBasicAmount { get; set; }
        //public decimal? ISMINC_TotalPercentage { get; set; }
        public List<ISM_Invoice_DetailsDMO> ISM_Invoice_DetailsDMO { get; set; }
        public List<ISM_Invoice_TaxDMO> ISM_Invoice_TaxDMO { get; set; }

        //ISMINC_InstallmentName
    }
}
