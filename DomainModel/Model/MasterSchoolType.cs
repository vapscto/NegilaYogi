using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_SchoolType")]
    public class MasterSchoolType : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMTYP_Id { get; set; }
        public string IVRMMTYP_Type { get; set; }
        public string IVRMMTYP_Description { get; set; }
        public bool Is_Active { get; set; }
    }
}
