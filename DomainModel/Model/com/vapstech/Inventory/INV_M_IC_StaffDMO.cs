using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_IC_Staff", Schema = "INV")]
    public class INV_M_IC_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMICST_Id { get; set; }
        public long INVMIC_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool INVMICST_ActiveFlg { get; set; }
        public INV_M_ItemConsumptionDMO INV_M_ItemConsumptionDMO { get; set; }



    }
}
