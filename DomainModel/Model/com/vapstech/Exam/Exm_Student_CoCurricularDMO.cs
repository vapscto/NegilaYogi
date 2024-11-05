using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_CoCurricular_Mapping", Schema = "Exm")]
    public class Exm_Student_CoCurricularDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ESCOM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int ECC_Id { get; set; }
        public int EME_Id { get; set; }
        public bool ESCOM_ActiveFlag { get; set; }
        public string ESCOM_Remarks { get; set; }
    }
}
