using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Menu_Page_Mapping")]
    public class IVRM_Master_Menu_Page_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMMPM_Id { get; set; }
        public long IVRMMM_Id { get; set; }
        public long IVRMP_Id { get; set; }
    }
}
