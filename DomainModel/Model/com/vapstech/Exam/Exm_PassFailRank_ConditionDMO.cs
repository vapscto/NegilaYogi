using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_PassFailRank_Condition", Schema = "Exm")]
    public class Exm_PassFailRank_ConditionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EPFRC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public int EME_Id { get; set; }
        public string EPFRC_ExamFlag { get; set; }
        public int EPFRC_From { get; set; }
        public int EPFRC_To { get; set; }
        public string EPFRC_Condition { get; set; }
        public int EPFRC_RankFlag { get; set; }
        public string EPFRC_PassFailFlag { get; set; }
        public bool EPFRC_ActiveFlag { get; set; }
        // public decimal? EPFRC_Percentage { get; set; }
        public decimal? EPFRC_OverallPercentage { get; set; }

    }
}
