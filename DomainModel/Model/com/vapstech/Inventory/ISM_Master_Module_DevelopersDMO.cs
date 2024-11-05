using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Master_Module_Developers")]
    public class ISM_Master_Module_DevelopersDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMMMDDE_Id { get; set; }
        public long ISMMMD_Id { get; set; }
        public long IVRMMMDDE_ModuleIncharge { get; set; }
        public bool IVRMMMDDE_ModuleHeadFlg { get; set; }
        public bool ISMMMDDE_ActiveFlag { get; set; }
        public long ISMMMDDE_CreatedBy { get; set; }
        public long ISMMMDDE_UpdatedBy { get; set; }
    }
}
