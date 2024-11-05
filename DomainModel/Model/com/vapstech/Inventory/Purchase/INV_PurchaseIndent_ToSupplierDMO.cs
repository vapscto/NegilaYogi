using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_PurchaseIndent_ToSupplier", Schema = "INV")]
    public class INV_PurchaseIndent_ToSupplierDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVPITS_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public string INVPITS_SupplierName { get; set; }
        public long INVPITS_ContactNo { get; set; }
        public string INVPITS_EmailId { get; set; }
        public DateTime INVPITS_SMSSentDate { get; set; }
        public DateTime INVPITS_MailSentDate { get; set; }
        public bool INVPITS_ActiveFlg { get; set; }
        public long INVPITS_CreatedBy { get; set; }
        public long INVPITS_UpdatedBy { get; set; }      

    }
}
