using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Group", Schema = "INV")]
    public class INV_Master_GroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMG_GroupName { get; set; }
        public string INVMG_AliasName { get; set; }
        public long INVMG_ParentId { get; set; }      
        public string INVMG_Level { get; set; }
        public string INVMG_MGUGIGFlg { get; set; }
        public string INVMG_GroupStartingNo { get; set; }
        public string INVMG_GroupSuffix { get; set; }
        public string INVMG_GroupPrefix { get; set; }
        public bool INVMG_ActiveFlg { get; set; }

    }
}
