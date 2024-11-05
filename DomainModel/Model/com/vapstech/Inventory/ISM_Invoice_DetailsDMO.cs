using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Invoice_Details")]
    public class ISM_Invoice_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMINCD_Id { get; set; }
        public long? ISMCLTC_Id { get; set; }
        public long? ISMCLTPRMP_Id { get; set; }
        public decimal ISMINCD_Qty { get; set; }
        public decimal ISMINCD_UnitRate { get; set; }
        public decimal ISMINCD_Amount { get; set; }
        public string ISMINCD_ItemDesc { get; set; }
        public string ISMINCD_Remarks { get; set; }
        public bool ISMINCD_ActiveFlag { get; set; }
        public DateTime ISMINCD_CreatedDate { get; set; }
        public long ISMINCD_CreatedBy { get; set; }
        public DateTime ISMINCD_UpdatedDate { get; set; }
        public long ISMINCD_UpdatedBy { get; set; }
        public long ISMINC_Id { get; set; }
        //public string ISMINCD_HSNCode { get; set; }
        //public string ISMINCD_SACCode { get; set; }
    }
}
