using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("IVRM_Student_User_Login")]
    public class StudentUserLoginDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMSTUUL_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMSTUUL_UserName { get; set; }
        public string IVRMSTUUL_Password { get; set; }
        public long AMST_Id { get; set; }
        public int IVRMSTUUL_ActiveFlag { get; set; }
        public string IVRMSTUUL_SecurityQns { get; set; }
        public string IVRMSTUUL_Answer { get; set; }


    }
}
