using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Item", Schema = "INV")]
    public class INV_Master_ItemDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMI_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMG_Id { get; set; }
    
        public long INVMUOM_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public decimal INVMI_MaxStock { get; set; }
        public bool INVMI_TaxAplFlg { get; set; }
        public string INVMI_ItemCode { get; set; }
        public bool INVMI_ActiveFlg { get; set; }
        public decimal INVMI_ReorderStock { get; set; }
        public bool INVMI_RawMatFlg { get; set; }
        public bool INVMI_ForSaleFlg { get; set; }
        public bool INVMI_MaintenanceAplFlg { get; set; }
        public string INVMI_HSNCode { get; set; }
        public string INVMI_GroupItemNo { get; set; }
        public List<INV_Master_Item_TaxDMO> INV_Master_Item_TaxDMO { get; set; }





    }
}
