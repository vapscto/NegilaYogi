using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Subject_Group_Exams", Schema = "Exm")]
    public class ExamSubjectGroupMappingExamsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ESGE_Id { get; set; }
        public int ESG_Id { get; set; }
        public int EME_Id { get; set; }
        public bool ESGE_ActiveFlag { get; set; }
       // public ExamsubjectGroupMappingDMO ExamsubjectGroupMappingDMO { get; set; }
    }
}
