using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_OpeningBalance", Schema = "INV")]
    public class INV_OpeningBalanceDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long IMFY_Id { get; set; }
        public string INVOB_BatchNo { get; set; }
        public DateTime? INVOB_PurchaseDate { get; set; }
        public decimal INVOB_PurchaseRate { get; set; }
        public decimal INVOB_SaleRate { get; set; }
    
    
        public decimal INVOB_Qty { get; set; }
        public string INVOB_Naration { get; set; }
        public DateTime? INVOB_MfgDate { get; set; }
        public DateTime? INVOB_ExpDate { get; set; }
        public bool INVOB_ActiveFlg { get; set; }


    }
}
