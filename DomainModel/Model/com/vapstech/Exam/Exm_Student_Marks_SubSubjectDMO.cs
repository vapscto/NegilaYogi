using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_Marks_SubSubject", Schema = "Exm")]
    public class Exm_Student_Marks_SubSubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ESTMSS_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("ESTM_Id")]
        public int ESTM_Id { get; set; }
        public int EMSS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public decimal? ESTMSS_Marks { get; set; }
        public string ESTMSS_MarksGradeFlg { get; set; }
        public string ESTMSS_Grade { get; set; }
        public long Login_Id { get; set; }
        public DateTime LoginDateTime { get; set; }
        public string IP4 { get; set; }
        public bool ESTMSS_ActiveFlg { get; set; }
        public string ESTMSS_Flg { get; set; }
        public long? ESTMSS_CreatedBy { get; set; }
        public long? ESTMSS_UpdatedBy { get; set; }

    }
}
