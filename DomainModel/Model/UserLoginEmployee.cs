using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_User_Login_Faculty")]
    public class UserLoginEmployee : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMULF_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long MI_Id { get; set; }
        public long Emp_Code { get; set; }
    }
}
