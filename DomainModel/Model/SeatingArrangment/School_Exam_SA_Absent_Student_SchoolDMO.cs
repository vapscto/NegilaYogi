using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Absent_Student_School")]
    public class School_Exam_SA_Absent_Student_SchoolDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAABSTUSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public DateTime? ESAABSTUSCH_ExamDate { get; set; }
        public bool ESAABSTUSCH_ActiveFlg { get; set; }
        public DateTime? ESAABSTUSCH_CreatedDate { get; set; }
        public DateTime? ESAABSTUSCH_UpdatedDate { get; set; }
        public long ESAABSTUSCH_CreatedBy { get; set; }
        public long ESAABSTUSCH_UpdatedBy { get; set; }
    }
}
