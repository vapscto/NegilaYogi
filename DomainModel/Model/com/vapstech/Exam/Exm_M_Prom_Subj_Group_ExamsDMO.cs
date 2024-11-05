using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_M_Prom_Subj_Group_Exams", Schema = "Exm")]
    public class Exm_M_Prom_Subj_Group_ExamsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EMPSGE_Id { get; set; }
        [ForeignKey("EMPSG_Id")]
        public int EMPSG_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal? EMPSGE_ForMaxMarkrs { get; set; }
        public bool EMPSGE_ActiveFlg { get; set; }
        public bool? EMPSGE_ConvertionReqOrNot { get; set; }
    }
}