using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Student_Groupwise_PC_Remarks  ", Schema = "Exm")]
    public class ExamPromotionGroupwiseRemarksDMO
       
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESGPCR_Id { get; set; }
        public long MI_ID { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_ID { get; set; }
        public long ASMS_ID { get; set; }
        public string EMPSG_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ESGPCR_Remarks { get; set; }
        public bool ESGPCR_ActiveFlag { get; set; }
        public DateTime? ESGPCR_CreatedDate { get; set; }
        public DateTime? ESGPCR_UpdatedDate { get; set; }
        public long ESGPCR_CreatedBy { get; set; }
        public long ESGPCR_UpdatedBy { get; set; }
        public string ESGPCR_Conduct { get; set; }


    }
}
