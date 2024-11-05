using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_M_RS_Subj_Group", Schema = "CLG")]
    public class Clg_Exm_M_RS_Subj_GroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EMRSSG_Id { get; set; }
        public long EMRSS_Id { get; set; }
        public string EMRSSG_GroupName { get; set; }
        public string EMRSSG_DisplayName { get; set; }
        public decimal? EMRSSG_PercentValue { get; set; }
        public decimal? EMRSSG_MarksValue { get; set; }
        public int EMRSSG_MaxOff { get; set; }
        public int EMRSSG_BestOff { get; set; }
        public bool EMRSSG_ActiveFlag { get; set; }
        public List<Clg_Exm_M_RS_Subj_Group_ExamsDMO> Clg_Exm_M_RS_Subj_Group_ExamsDMO { get; set; }

    }
}
