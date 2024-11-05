using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Module")]

    public class MasterModules : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }

        public int Module_ActiveFlag { get; set; }

        public long userid { get; set; }
    }
}
