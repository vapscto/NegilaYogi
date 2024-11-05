using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Institution_Module")]
    public class Institution_Module : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMIM_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public int IVRMIM_Flag { get; set; }
        public int IVRMIM_ModuleOrder { get; set; }
        public string IVRMIM_CDFlag { get; set; }

    }
}
