using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Yearly_Category", Schema = "Exm")]
    public class Exm_Yearly_CategoryDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public int EYC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
       public bool? EYC_BasedOnPaperTypeFlg { get; set; }
        public bool EYC_ActiveFlg { get; set; }
        public DateTime? EYC_ExamStartDate { get; set; }
        public DateTime? EYC_ExamEndDate { get; set; }
        public DateTime? EYC_MarksEntryLastDate { get; set; }
        public DateTime? EYC_MarksProcessLastDate { get; set; }
        public DateTime? EYC_MarksPublishDate { get; set; }
        public Exm_Yearly_Category_GroupDMO Exm_Yearly_Category_Groups { get; set; }
    }
}
