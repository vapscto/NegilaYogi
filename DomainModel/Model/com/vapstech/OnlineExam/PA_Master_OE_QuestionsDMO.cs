using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Master_OE_Questions")]
    public class PA_Master_OE_QuestionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMOEQ_Id { get; set; }       
        public long MI_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string PAMOEQ_Question { get; set; }
        public decimal? PAMOEQ_Marks { get; set; }
        public string PAMOEQ_QuestionDesc { get; set; }
        public List<PA_Master_OE_Questions_FilesDMO> PA_Master_OE_Questions_FilesDMO { get; set; }
    }
}
