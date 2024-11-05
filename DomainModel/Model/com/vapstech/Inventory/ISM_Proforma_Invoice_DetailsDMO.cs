using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Proforma_Invoice_Details")]
    public class ISM_Proforma_Invoice_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMPRINCD_Id { get; set; }
        public long ISMPRINC_Id { get; set; }
        public long? ISMCLTC_Id { get; set; }
        public long? ISMCLTPRMP_Id { get; set; }
        public decimal ISMPRINCD_Qty { get; set; }
        public decimal ISMPRINCD_UnitRate { get; set; }
        public decimal ISMPRINCD_Amount { get; set; }
        public string ISMPRINCD_ItemDesc { get; set; }
        public string ISMPRINCD_Remarks { get; set; }
        public bool ISMPRINCD_ActiveFlag { get; set; }
        public DateTime ISMPRINCD_CreatedDate { get; set; }
        public long ISMPRINCD_CreatedBy { get; set; }
        public DateTime ISMPRINCD_UpdatedDate { get; set; }
        public long ISMPRINCD_UpdatedBy { get; set; }
        //public string ISMPRINCD_HSNCode { get; set; }
        //public string ISMPRINCD_SACCode { get; set; }
    }
}
