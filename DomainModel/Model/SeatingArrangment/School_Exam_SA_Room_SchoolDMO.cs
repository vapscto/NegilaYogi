using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Room_School")]
    public class School_Exam_SA_Room_SchoolDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESARMSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public DateTime? ESARMSCH_ExamDate { get; set; }
        public long ESAROOM_Id { get; set; }
        public bool ESARMSCH_ActiveFlg { get; set; }
        public DateTime? ESARMSCH_CreatedDate { get; set; }
        public DateTime? ESARMSCH_UpdatedDate { get; set; }
        public long ESARMSCH_CreatedBy { get; set; }
        public long ESARMSCH_UpdatedBy { get; set; }
        public List<School_Exam_SA_Room_ClassSubject_SchoolDMO> School_Exam_SA_Room_ClassSubject_SchoolDMO { get; set; }
    }
}
