using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Exam.StudentMentorMapping
{
    [Table("Adm_Master_Mentor_College", Schema = "CLG")]
    public class Clg_Student_Mentor_UserDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMMEC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }      
        public long HRME_Id { get; set; }
        public bool AMMEC_Activeflag { get; set; }
        public long AMMEC_CreatedBy { get; set; }
        public long AMMEC_UpdatedBy { get; set; }
        public List<Colleg_Student_Mentor_DetailsDMO> Colleg_Student_Mentor_DetailsDMO { get; set; }
    }
}
