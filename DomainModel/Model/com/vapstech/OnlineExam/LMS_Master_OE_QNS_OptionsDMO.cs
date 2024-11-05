using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Master_OE_QNS_Options")]
    public class LMS_Master_OE_QNS_OptionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSMOEQOA_Id { get; set; }
        [ForeignKey("LMSMOEQ_Id")]
        public long LMSMOEQ_Id { get; set; }
        public string LMSMOEQOA_Option { get; set; }
        public string LMSMOEQOA_OptionCode { get; set; }
        // public string PAMOEAO_OptionsFlag { get; set; }      
        public bool LMSMOEQOA_AnswerFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        //  public List<LMS_Master_OE_QuestionsDMO> LMS_Master_OE_QuestionsDMO { get; set; }

    }
}
