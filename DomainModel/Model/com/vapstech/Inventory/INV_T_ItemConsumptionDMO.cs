using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_ItemConsumption", Schema = "INV")]
    public class INV_T_ItemConsumptionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTIC_Id { get; set; }
        public long INVMIC_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVMP_Id { get; set; }
        public string INVTIC_BatchNo { get; set; }
        public decimal INVTIC_ICPrice { get; set; }
        public decimal INVTIC_ICQty { get; set; }
        public string INVTIC_Naration { get; set; }
        public bool INVTIC_ActiveFlg { get; set; }
        public INV_M_ItemConsumptionDMO INV_M_ItemConsumptionDMO { get; set; }


    }
}
