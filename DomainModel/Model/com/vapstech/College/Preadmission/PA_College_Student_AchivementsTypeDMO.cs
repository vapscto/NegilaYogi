using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_AchivementsType", Schema = "CLG")]
    public class PA_College_Student_AchivementsTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long PACSAT_Id { get; set; }
        public long PACA_Id { get; set; }
        public string PACSAT_AchivementsName { get; set; }
        public string PACSAT_type { get; set; }
        public string PACSAT_Filename { get; set; }
        public string PACSAT_Filepath { get; set; }

    }
}
