using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_IC_Student", Schema = "INV")]
    public class INV_M_IC_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMICS_Id { get; set; }
        public long INVMIC_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool INVMICS_ActiveFlg { get; set; }

        public INV_M_ItemConsumptionDMO INV_M_ItemConsumptionDMO { get; set; }
    }
}
