using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Students_Exam_SubjectiveAnswer_Files")]
    public class LP_Students_Exam_SubjectiveAnswer_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPSTUEXSANSFL_Id { get; set; }
        public long LPSTUEXSANS_Id { get; set; }
        public string LPSTUEXSANSFL_FileName { get; set; }
        public string LPSTUEXSANSFNFL_FilePath { get; set; }
        public bool? LPSTUEXSANSFL_ActiveFlg { get; set; }
        public long? LPSTUEXSANSFL_CreatedBy { get; set; }
        public DateTime? LPSTUEXSANSFL_CreatedDate { get; set; }
        public long? LPSTUEXSANSFL_UpdatedBy { get; set; }
        public DateTime? LPSTUEXSANSFL_UpdatedDate { get; set; }
    }
}
