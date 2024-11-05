using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Subjectwise_Written_Marks_Students")]
    public class WrittenTestStudentSubjectWiseMarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASWMS_Id { get; set; }
        public long MI_Id { get; set; }
        public long PASWM_Id { get; set; }
        public long PASR_Id { get; set; }
        public decimal PASWMS_MarksScored { get; set; }
        public string PASWMS_PassFail { get; set; }
    }
}
