using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{

    [Table("EXM_CCE_Activities_AREA", Schema = "Exm")]
    public class EXM_CCE_Activities_AREADMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ECACTA_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECACTA_SkillArea { get; set; }
        public int ECACTA_SkillOrder { get; set; }
        public bool ECACTA_ActiveFlag { get; set; }

    }
}




