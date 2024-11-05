using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Exam.StudentMentorMapping
{
    [Table("Adm_Master_Mentor_College_Mentee", Schema = "CLG")]
   public class Colleg_Student_Mentor_DetailsDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMMECM_Id { get; set; }
        public long AMMEC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool AMMECM_Activeflag { get; set; }
        public long AMMECM_CreatedBy { get; set; }
        public long AMMECM_UpdatedBy { get; set; }
    }
}
