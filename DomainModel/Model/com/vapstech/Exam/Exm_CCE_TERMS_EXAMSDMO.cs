using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_CCE_TERMS_EXAMS", Schema = "Exm")]
    public class Exm_CCE_TERMS_EXAMSDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECTEX_Id { get; set; }
        public int ECT_Id { get; set; }
        public int EME_Id { get; set; }
        public bool ECTEX_RoundOffReqFlg { get; set; }
        public bool ECTEX_ConversionReqFlg { get; set; }
        public decimal? ECTEX_MarksPercentValue { get; set; }
        public string ECTEX_MarksPerFlag { get; set; }
        public bool? ECTEX_ActiveFlag { get; set; }
        public bool? ECTEX_NotApplToTotalFlg { get; set; }

    }
}
