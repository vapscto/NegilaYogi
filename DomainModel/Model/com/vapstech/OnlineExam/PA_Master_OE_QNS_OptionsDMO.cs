using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Master_OE_QNS_Options")]
    public class PA_Master_OE_QNS_OptionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMOEQOA_Id { get; set; }       
        public long PAMOEQ_Id { get; set; }
        public string PAMOEQOA_Option { get; set; }
        public string PAMOEQOA_OptionCode { get; set; }
        public bool PAMOEQOA_AnswerFlag { get; set; }        
    }
}
