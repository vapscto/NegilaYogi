using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Role_Type")]
    public class MasterRoleType : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMRT_Id { get; set; }
        public string IVRMRT_Role { get; set; }
        public long IVRMR_Id { get; set; }
        public string IVRMRT_RoleFlag { get; set; }
        public string flag { get; set; }
    }
}
