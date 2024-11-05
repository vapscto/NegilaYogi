using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_Marks", Schema = "Exm")]
    public class ExamMarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal ESTM_Marks { get; set; }
        public string ESTM_MarksGradeFlg { get; set; }
        public long Id { get; set; }
        public DateTime LoginDateTime { get; set; }
        public string IP4 { get; set; }
        public bool ESTM_ActiveFlg { get; set; }
        public string ESTM_Grade { get; set; }     
        public string ESTM_Flg { get; set; }
        public List<Exm_Student_Marks_SubSubjectDMO> Exm_Student_Marks_SubSubjectDMO { get; set; }
        public long? ESTM_CreatedBy { get; set; }
        public long? ESTM_UpdatedBy { get; set; }
        public bool? ESTM_OnlineExamFlag { get; set; }

    }
}
