using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{  
    [Table("EXM_CCE_SKILLS_AREA_Mapping", Schema = "Exm")]
    public class CCE_Master_Life_Skill_Areas_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ECSAM_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public int ECS_Id { get; set; }
        public int ECSA_Id { get; set; }
        public string ECSAM_IndicatorDescription { get; set; }
        public int EMGR_Id { get; set; }
        public bool ECSAM_ActiveFlag { get; set; }
        //public int EMGD_Id { get; set; }
    }
}
