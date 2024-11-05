using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_Exam", Schema = "Exm")]
    public class exammasterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public string EME_ExamDescription { get; set; }
        public string EME_IVRSExamName { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }
    }
}
