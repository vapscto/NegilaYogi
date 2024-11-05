using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Product_Stages", Schema = "DCS")]
    public class INV_Master_Product_StagesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVMPS_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMP_Id { get; set; }
        public string INVMPS_Stages { get; set; }
        public bool INVMPS_ActiveFlg { get; set; }

    }
}
