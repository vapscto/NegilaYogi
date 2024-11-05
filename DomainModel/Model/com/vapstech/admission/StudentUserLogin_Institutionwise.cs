using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("IVRM_Student_User_Login_Institutionwise")]
    public class StudentUserLogin_Institutionwise:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMSTUULI_Id { get; set; }
        public long IVRMSTUUL_Id { get; set; }
        public long AMST_Id { get; set; }
        public int IVRMSTUULI_ActiveFlag { get; set; }
        public long Previous_AMST_Id { get; set; }
        public long Previous_MI_Id { get; set; }
    }
}
