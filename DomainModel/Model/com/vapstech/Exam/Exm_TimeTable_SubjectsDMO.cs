using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_TimeTable_Subjects", Schema = "Exm")]
    public class Exm_TimeTable_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EXTTS_Id { get; set; }
        public int EXTT_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ETTS_Id { get; set; }
        public DateTime? EXTTS_Date { get; set; }
        public string EXTTS_ExamDuration { get; set; }
        public string EXTTS_FromTime { get; set; }
        public string EXTTS_EndTime { get; set; }
        public bool EXTTS_ActiveFlag { get; set; }

    }
}
