using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Subject_Group_Subjects", Schema = "Exm")]
    public class ExamSubjectGroupMappingSubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESGS_Id { get; set; }
        public int ESG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ESGS_ActiveFlag { get; set; }
       // public ExamsubjectGroupMappingDMO ExamsubjectGroupMappingDMO { get; set; }
    }
}
