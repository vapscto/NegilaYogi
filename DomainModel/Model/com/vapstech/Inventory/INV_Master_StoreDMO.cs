using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Store", Schema = "INV")]
    public class INV_Master_StoreDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMS_StoreLocation { get; set; }
        public bool INVMS_ActiveFlg { get; set; }
        public string INVMS_ContactPerson { get; set; }
        public long INVMS_ContactNo { get; set; }
        public long HRME_Id { get; set; }




    }
}
