using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Exam.StudentMentor
{
    [Table("Adm_Master_Mentor")]
    public class School_Adm_Master_MentorDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long AMME_CreatedBy { get; set; }
        public long AMME_UpdatedBy { get; set; }
        public List<School_Adm_Master_Mentor_MenteeDMO> School_Adm_Master_Mentor_MenteeDMO { get; set; }
    }
}
