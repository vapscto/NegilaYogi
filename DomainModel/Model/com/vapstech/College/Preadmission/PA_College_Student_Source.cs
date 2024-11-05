using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_Source", Schema = "CLG")]
    public class PA_College_Student_Source : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PACSTS_Id { get; set; }
        public long PACA_Id { get; set; }
        public long ASRS_Id { get; set; }

    }
}
