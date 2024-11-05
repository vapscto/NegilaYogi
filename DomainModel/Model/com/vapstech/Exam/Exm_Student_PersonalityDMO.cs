using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_Personality_Mapping", Schema = "Exm")]
    public class Exm_Student_PersonalityDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public int ESPM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EP_Id { get; set; }
        public int EME_Id { get; set; }
        public string ESPM_Remarks { get; set; }
        public bool ESPM_ActiveFlag { get; set; }
    }
}
