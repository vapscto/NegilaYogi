using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_ItemConsumption", Schema = "INV")]
    public class INV_M_ItemConsumptionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMIC_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMIC_StuOtherFlg { get; set; }
        public string INVMIC_ICNo { get; set; }
        public DateTime INVMIC_ICDate { get; set; }
        public string INVMIC_Remarks { get; set; }
        public bool INVMIC_ActiveFlg { get; set; }

        public List<INV_T_ItemConsumptionDMO> INV_T_ItemConsumptionDMO { get; set; }
        public List<INV_M_IC_StaffDMO> INV_M_IC_StaffDMO { get; set; }
        public List<INV_M_IC_DepartmentDMO> INV_M_IC_DepartmentDMO { get; set; }
        public List<INV_M_IC_StudentDMO> INV_M_IC_StudentDMO { get; set; }
        public List<NV_M_ItemConsumptionCLGDMO> NV_M_ItemConsumptionCLGDMO { get; set; }



    }
}
