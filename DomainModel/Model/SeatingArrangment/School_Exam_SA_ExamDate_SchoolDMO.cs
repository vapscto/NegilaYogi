using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ExamDate_School")]
    public class School_Exam_SA_ExamDate_SchoolDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ESAEDATESCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public DateTime ESAEDATESCH_ExamDate { get; set; }
        public bool ESAEDATESCH_ActiveFlg { get; set; }
        public DateTime? ESAEDATESCH_CreatedDate { get; set; }
        public DateTime? ESAEDATESCH_UpdatedDate { get; set; }
        public long ESAEDATESCH_CreatedBy { get; set; }
        public long ESAEDATESCH_UpdatedBy { get; set; }
        public List<School_Exam_SA_ExamDate_Room_SchoolDMO> School_Exam_SA_ExamDate_Room_SchoolDMO { get; set; }

    }
}
