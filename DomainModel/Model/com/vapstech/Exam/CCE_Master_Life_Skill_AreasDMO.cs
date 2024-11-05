using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_CCE_SKILLS_AREA", Schema = "Exm")] 
    public class CCE_Master_Life_Skill_AreasDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public int ECSA_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECSA_SkillArea { get; set; }
        public int ECSA_SkillOrder { get; set; }
        public bool ECSA_ActiveFlag { get; set; }

    }
}
