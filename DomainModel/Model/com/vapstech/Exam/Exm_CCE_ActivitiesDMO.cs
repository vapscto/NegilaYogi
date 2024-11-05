using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{

    [Table("Exm_CCE_Activities", Schema = "Exm")]
    public class Exm_CCE_ActivitiesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ECACT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECACT_SkillName { get; set; }
        public string ECACT_SkillCode { get; set; }
        public bool ECACT_ActiveFlag { get; set; }

    }
}




