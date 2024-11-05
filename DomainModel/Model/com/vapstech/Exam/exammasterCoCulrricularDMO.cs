using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_CoCurricular", Schema = "Exm")]
    public class exammasterCoCulrricularDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public int ECC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECC_CoCurricularName { get; set; }
        public string ECC_CoCurricularCode { get; set; }
        public long ECC_CoCurricularOrder { get; set; }
        public bool ECC_ActiveFlag { get; set; }
    }
}
