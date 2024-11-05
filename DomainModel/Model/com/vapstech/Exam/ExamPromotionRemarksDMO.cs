using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Promotion_Remarks_Details", Schema = "Exm")]
    public class ExamPromotionRemarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EPRD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public string EPRD_PromotionName { get; set; }
        public string EPRD_Remarks { get; set; }
        public string EPRD_ClassPromoted { get; set; }
        public string EPRD_Promotionflag { get; set; }
    }
}
