using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Students_Exam_SubjectiveAnswer_Staff_Files")]
    public class LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPSTUEXSANSSFL_Id { get; set; }
        public long LPSTUEXSANS_Id { get; set; }
        public string LPSTUEXSANSSFL_FileName { get; set; }
        public string LPSTUEXSANSSFL_FilePath { get; set; }
        public bool? LPSTUEXSANSSFL_ActiveFlg { get; set; }
        public long? LPSTUEXSANSSFL_CreatedBy { get; set; }
        public DateTime? LPSTUEXSANSSFL_CreatedDate { get; set; }
        public long? LPSTUEXSANSSFL_UpdatedBy { get; set; }
        public DateTime? LPSTUEXSANSSFL_UpdatedDate { get; set; }
    }
}
