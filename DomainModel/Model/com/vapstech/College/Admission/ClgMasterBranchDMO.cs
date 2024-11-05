using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Master_Branch", Schema ="CLG")]
    public class ClgMasterBranchDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMB_Id { get; set; }    
        public long MI_Id { get; set; }
        public string  AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public string AMB_BranchInfo { get; set; }
        public string AMB_BranchType { get; set; }
        public int AMB_StudentCapacity { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public string AMB_AidedUnAided { get; set; }
    }
}
