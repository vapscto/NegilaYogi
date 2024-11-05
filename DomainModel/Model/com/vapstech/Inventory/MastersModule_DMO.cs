using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Master_Module")]
    public class MastersModule_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMMD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public long ISMMMD_ModuleHeadId { get; set; }
        public bool ISMMMD_ActiveFlag { get; set; }
        public long ISMMMD_CreatedBy { get; set; }
        public long ISMMMD_UpdatedBy { get; set; }

        public List<ISM_Master_Module_DevelopersDMO> ISM_Master_Module_DevelopersDMO { get; set; }
    }
}
