using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Master_OE_Questions")]
    public class LMS_Master_OE_QuestionsDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSMOEQ_Id { get; set; }       
        public long MI_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string LMSMOEQ_Question { get; set; }
        public decimal LMSMOEQ_Marks { get; set; }
        public string LMSMOEQ_QuestionDesc { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<LMS_Master_OE_Questions_FilesDMO> LMS_Master_OE_Questions_FilesDMO { get; set; }
    }
}
