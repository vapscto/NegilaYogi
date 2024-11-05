using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_University_Exam")]
    public class Exam_SA_University_ExamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAUE_Id { get; set; }
        public long MI_Id { get; set; }
        public string ESAUE_ExamName { get; set; }
        public string ESAUE_ExamCode { get; set; }
        public int ESAUE_ExamOrder { get; set; }
        public bool ESAUE_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long ESAUE_CreatedBy { get; set; }
        public long ESAUE_UpdatedBy { get; set; }

    }
}
