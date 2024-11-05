using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Module")]
    public class MasterModule : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }

        public int Module_ActiveFlag { get; set; }
        public long userid { get; set; }
        public string IVRMM_Flag { get; set; }
        public string IVRMM_ModuleIconPic { get; set; }

       // public ICollection<pagemodulemapping> modulepagemapping { get; set; }
    }
}
