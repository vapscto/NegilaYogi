using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Student_SubjEx_PC_Remarks", Schema = "Exm")]
    public class Exm_Student_ProgressCard_SubjectWise_RemarksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ESSEPCR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EME_ID { get; set; }
        public long ISMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ESSEPCR_Remarks { get; set; }
        public bool? ESSEPCR_ActiveFlag { get; set; }
        public DateTime? ESSEPCR_CreatedDate { get; set; }
        public DateTime? ESSEPCR_UpdatedDate { get; set; }
        public long? ESSEPCR_CreatedBy { get; set; }
        public long? ESSEPCR_UpdatedBy { get; set; }
    }
}
