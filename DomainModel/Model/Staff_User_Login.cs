using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Staff_User_Login")]
    
    public class Staff_User_Login : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IVRMSTAUL_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMSTAUL_UserName { get; set; }
        public string IVRMSTAUL_Password { get; set; }
       
        public long Emp_Code { get; set; }
        public int IVRMSTAUL_ActiveFlag { get; set; }
        public long Id { get; set; }

    }
}
