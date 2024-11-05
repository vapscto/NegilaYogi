using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_IC_Department", Schema = "INV")]
    public class INV_M_IC_DepartmentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMICD_Id { get; set; }
        public long INVMIC_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool INVMICD_ActiveFlg { get; set; }
        public INV_M_ItemConsumptionDMO INV_M_ItemConsumptionDMO { get; set; }

    }
}
