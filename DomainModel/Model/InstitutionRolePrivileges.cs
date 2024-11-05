using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Institution_Role_Privileges")]
    public class InstitutionRolePrivileges : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMIRP_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long IVRMIMP_Id { get; set; }
        public bool IVRMIRP_AddFlag { get; set; }
        public bool IVRMIRP_UpdateFlag { get; set; }
        public bool IVRMIRP_DeleteFlag { get; set; }
        public bool IVRMIRP_ReportFlag { get; set; }
        public bool IVRMIRP_SearchFlag { get; set; }
        public bool IVRMIRP_ProcessFlag { get; set; }
    }
}
