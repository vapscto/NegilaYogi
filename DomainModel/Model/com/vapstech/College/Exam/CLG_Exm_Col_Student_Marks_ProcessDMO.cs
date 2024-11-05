using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Student_Marks_Process", Schema = "CLG")]
    public class CLG_Exm_Col_Student_Marks_ProcessDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECSTMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal ECSTMP_TotalMaxMarks { get; set; }
        public decimal ECSTMP_TotalObtMarks { get; set; }
        public decimal ECSTMP_Percentage { get; set; }
        public string ECSTMP_TotalGrade { get; set; }
        public int ECSTMP_SemRank { get; set; }
        public int ECSTMP_SectionRank { get; set; }
        public string ECSTMP_Result { get; set; }
    }
}
