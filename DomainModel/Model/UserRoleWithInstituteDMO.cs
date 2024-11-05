using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_User_Login_Institutionwise")]
    public class UserRoleWithInstituteDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMULI_Id { get; set; }
        public long MI_Id { get; set; }
        public int Id { get; set; }
        public int Activeflag { get; set; } 

    }
}
