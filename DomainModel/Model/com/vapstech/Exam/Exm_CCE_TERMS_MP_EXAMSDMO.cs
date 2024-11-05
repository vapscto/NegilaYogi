using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{

    [Table("Exm_CCE_TERMS_MP_EXAMS", Schema = "Exm")]
    public class Exm_CCE_TERMS_MP_EXAMSDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ECTMPE_Id { get; set; }
        public int ECTMP_Id { get; set; }
        public int? EME_ID { get; set; }       
        public bool? ECTMPE_ActiveFlag { get; set; }

    }
}




