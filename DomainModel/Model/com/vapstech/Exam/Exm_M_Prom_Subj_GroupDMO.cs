using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_M_Prom_Subj_Group", Schema = "Exm")]
    public class Exm_M_Prom_Subj_GroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EMPSG_Id { get; set; }
        [ForeignKey("EMPS_Id")]
        public int EMPS_Id { get; set; }
        public string EMPSG_GroupName { get; set; }
        public string EMPSG_DisplayName { get; set; }
        public decimal? EMPSG_PercentValue { get; set; }
        public decimal? EMPSG_MarksValue { get; set; }
        public int EMPSG_MaxOff { get; set; }
        public int EMPSG_BestOff { get; set; }
        public bool EMPSG_ActiveFlag { get;set;}
        public int? EMPSG_Order { get; set; }
        public bool? EMPSG_RoundOffFlag { get; set; }
        public List<Exm_M_Prom_Subj_Group_ExamsDMO> Exm_M_Prom_Subj_Group_Exams { get; set; }
    }
}
