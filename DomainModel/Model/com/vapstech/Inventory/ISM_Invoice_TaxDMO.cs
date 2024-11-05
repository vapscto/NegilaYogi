using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Invoice_Tax")]
    public class ISM_Invoice_TaxDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMINTX_Id { get; set; }
        public long ISMINC_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal ISMINTX_TaxPercent { get; set; }
        public decimal ISMINTX_TaxAmount { get; set; }
        public bool ISMINTX_ActiveFlag { get; set; }
        public DateTime ISMINTX_CreatedDate { get; set; }
        public long ISMINTX_CreatedBy { get; set; }
        public DateTime ISMINTX_UpdatedDate { get; set; }
        public long ISMINTX_UpdatedBy { get; set; }
    }
}
