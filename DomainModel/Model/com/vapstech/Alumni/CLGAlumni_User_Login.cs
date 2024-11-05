using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("IVRM_College_User_Login_Alumni", Schema = "CLG")]
    public class CLGAlumni_User_LoginDMO  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMCULAL_Id { get; set; }
        public long IVRMUL_Id { get; set; }
       
        public long ALCSREG_Id { get; set; }

    }
}
