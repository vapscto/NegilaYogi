using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_TimeTable", Schema = "Exm")]
    public class Exm_TimeTableDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EXTT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EME_Id { get; set; }
        public int EMG_Id { get; set; }
        public bool EXTT_ActiveFlag { get; set; }
        public DateTime? EXTT_FromDate { get; set; }
        public DateTime? EXTT_EndDate { get; set; }
        public List<Exm_TimeTable_SubjectsDMO> Exm_TimeTable_SubjectsDMO { get; set; }


    }
}
