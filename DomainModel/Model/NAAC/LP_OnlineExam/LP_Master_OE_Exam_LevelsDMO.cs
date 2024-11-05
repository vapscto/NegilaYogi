using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam_Levels")]
    public class LP_Master_OE_Exam_LevelsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEEXLVL_Id { get; set; }
        public long LPMOEEX_Id { get; set; }
        public string LPMOEEXLVL_LevelDesc { get; set; }
        public long? LPMOEEXLVL_TotalNoOfQns { get; set; }
        public long? LPMOEEXLVL_MaxQns { get; set; }
        public decimal? LPMOEEXLVL_LevelTotalMarks { get; set; }
        public decimal? LPMOEEXLVL_MarksPerQns { get; set; }
        public bool LPMOEEXLVL_ActiveFlg { get; set; }
        public long? LPMOEEXLVL_CreatedBy { get; set; }
        public DateTime? LPMOEEXLVL_CreatedDate { get; set; }
        public long? LPMOEEXLVL_UpdatedBy { get; set; }
        public DateTime? LPMOEEXLVL_UpdatedDate { get; set; }
        public long? LPMOEEXLVL_LevelOrder { get; set; }
        public List<LP_Master_OE_Exam_QuestionsDMO> LP_Master_OE_Exam_QuestionsDMO { get; set; }
    }
}
