using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Master_OE_Questions_Class")]
    public class LMS_Master_OE_Questions_ClassDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSMOEQC_Id { get; set; }
        [ForeignKey("LMSMOEQ_Id")]
        public long LMSMOEQ_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        //  public List<LMS_Master_OE_QuestionsDMO> LMS_Master_OE_QuestionsDMO { get; set; }
    }
}
