using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Module_Category")]
    public class ModulePage_Category : CommonParamDMO
    {
        [Key]
        public long IVRM_Module_Category_Id { get; set; }
        public long IVRM_MId { get; set; }
        public string Module_Category_Name { get; set; }
    }
}
