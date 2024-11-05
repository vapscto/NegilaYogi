using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ExamDate_Room_School")]
    public class School_Exam_SA_ExamDate_Room_SchoolDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAEDATERSCH_Id { get; set; }
        public long ESAEDATESCH_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public bool ESAEDATERSCH_ActiveFlg { get; set; }
        public DateTime? ESAEDATERSCH_CreatedDate { get; set; }
        public DateTime? ESAEDATERSCH_UpdatedDate { get; set; }
        public long ESAEDATERSCH_CreatedBy { get; set; }
        public long ESAEDATERSCH_UpdatedBy { get; set; }
    }
}
