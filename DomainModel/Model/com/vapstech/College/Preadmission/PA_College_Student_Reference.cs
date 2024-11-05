using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_Reference", Schema = "CLG")]
    public class PA_College_Student_Reference : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PASTR_Id { get; set; }
        public long PACA_Id { get; set; }
        public long ASRR_Id { get; set; }
    }
}
