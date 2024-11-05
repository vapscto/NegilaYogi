using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_PrevExtracurricular", Schema = "CLG")]
    public class PA_College_Student_PrevExtracurricularDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PACSPER_Id { get; set; }
        public long PACA_Id { get; set; }
        public string PACSPER_Type { get; set; }
        public string PACSPER_ActivityName { get; set; }

    }
}
