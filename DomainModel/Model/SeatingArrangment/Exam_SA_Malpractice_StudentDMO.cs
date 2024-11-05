using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Malpractice_Student")]
    public class Exam_SA_Malpractice_StudentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAMALSTU_Id { get; set; }
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
        public string ESAMALSTU_LABTHEORYFlg { get; set; }
        public string ESAMALSTU_StudentUSN { get; set; }
        public DateTime ESAMALSTU_ExamDate { get; set; }
        public bool ESAMALSTU_ActiveFlg { get; set; }
        public DateTime? ESAMALSTU_CreatedDate { get; set; }
        public DateTime? ESAMALSTU_UpdatedDate { get; set; }
        public long ESAMALSTU_CreatedBy { get; set; }
        public long ESAMALSTU_UpdatedBy { get; set; }

    }
}
