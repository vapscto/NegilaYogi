using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("IVRM_User_Login_Alumni", Schema = "ALU")]
    public class Alumni_User_LoginDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMULAL_Id  { get; set; }
        public long IVRMUL_Id { get; set; }
       
        public long ALSREG_Id { get; set; }

    }
}
