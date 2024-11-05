using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_StudentWiseSubjectGroupMarks", Schema = "Exm")]
    public class Exm_StudentWiseSubjectGroupMarksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESWSGM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int ESG_Id { get; set; }
        public string ESWSGM_SubjectGroupName { get; set; }
        public decimal? ESWSGM_GroupMaxMarks { get; set; }
        public decimal? ESWSGM_GroupObtainedMarks { get; set; }
    }
}