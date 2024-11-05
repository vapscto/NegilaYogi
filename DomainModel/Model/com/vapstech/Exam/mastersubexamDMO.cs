using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_SubExam", Schema = "Exm")]
    public class mastersubexamDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EMSE_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMSE_SubExamName { get; set; }
        public string EMSE_SubExamCode { get; set; }
        public int EMSE_SubExamOrder { get; set; }
        public bool EMSE_ActiveFlag { get; set; }

    }
}
