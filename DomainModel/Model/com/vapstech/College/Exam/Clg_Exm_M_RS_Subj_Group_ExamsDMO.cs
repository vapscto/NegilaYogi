using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_M_RS_Subj_Group_Exams", Schema = "CLG")]
    public class Clg_Exm_M_RS_Subj_Group_ExamsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EMRSSGE_Id { get; set; }
        public long EMRSSG_Id { get; set; }
        public int EME_Id { get; set; }
        public bool EMRSSGE_ActiveFlg { get; set; }

    }
}
