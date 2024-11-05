using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_UOM", Schema = "INV")]
    public class INV_Master_UOMDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMUOM_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public bool INVMUOM_ActiveFlg { get; set; }      
        public string INVMUOM_Qty { get; set; }


    }
}
