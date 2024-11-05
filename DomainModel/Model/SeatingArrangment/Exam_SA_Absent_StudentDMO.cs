using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Absent_Student")]
    public class Exam_SA_Absent_StudentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAABSTU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public string ESAABSTU_LABTheoryFlg { get; set; }
        public string ESAABSTU_StudentUSN { get; set; }
        public DateTime ESAABSTU_ExamDate { get; set; }
        public bool ESAABSTU_ActiveFlg { get; set; }
        public DateTime? ESAABSTU_CreatedDate { get; set; }
        public DateTime? ESAABSTU_UpdatedDate { get; set; }
        public long ESAABSTU_CreatedBy { get; set; }
        public long ESAABSTU_UpdatedBy { get; set; }

    }
}
