using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Proforma_Invoice_Tax")]
    public class ISM_Proforma_Invoice_TaxDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long ISMTTX_Id { get; set; }
        public long ISMPRINC_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal ISMMTTX_TaxPercent { get; set; }
        public decimal ISMMTTX_TaxAmount { get; set; }
        public bool ISMMTTX_ActiveFlag { get; set; }
        public DateTime ISMMTTX_CreatedDate { get; set; }
        public long ISMMTTX_CreatedBy { get; set; }
        public DateTime ISMMTTX_UpdatedDate { get; set; }
        public long ISMMTTX_UpdatedBy { get; set; }
    }
}
