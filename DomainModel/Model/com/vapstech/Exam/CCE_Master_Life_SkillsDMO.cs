using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
   
    [Table("EXM_CCE_SKILLS", Schema = "Exm")]
    public class CCE_Master_Life_SkillsDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public int ECS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECS_SkillName { get; set; }
        public string ECS_SkillCode { get; set; }        
        public bool ECS_ActiveFlag { get; set; }
        
    }
}
