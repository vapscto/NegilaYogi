using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_M_Promotion_Subjects", Schema = "Exm")]
    public class Exm_M_Promotion_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EMPS_Id { get; set; }
        [ForeignKey("EMP_Id")]
        public int EMP_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EMPS_MaxMarks { get; set; }
        public decimal? EMPS_MinMarks { get; set; }
        public decimal? EMPS_ConvertForMarks { get; set; }
        public bool EMPS_AppToResultFlg { get; set; }
        public bool EMPS_ActiveFlag { get; set; }
        public int? EMPS_SubjOrder { get; set; }       
        public List<Exm_M_Prom_Subj_GroupDMO> Exm_M_Prom_Subj_Group { get; set; }
    }
}
