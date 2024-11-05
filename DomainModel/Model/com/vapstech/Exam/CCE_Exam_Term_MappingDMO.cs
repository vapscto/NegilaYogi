using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{

    [Table("Exm_CCE_TERMS_MP", Schema = "Exm")]
    public class CCE_Exam_Term_MappingDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ECTMP_Id { get; set; }        
        public long ASMAY_Id { get; set; }        
        public int EMCA_Id { get; set; }
        public int ECT_Id { get; set; }       
        public string ECTMP_Name { get; set; }
        public decimal ECTMP_MarksPercentValue { get; set; }
        public string ECTMP_MarksPerFlag { get; set; }
        public bool? ECTMP_ActiveFlag { get; set; }
        public DateTime? ECTMP_TermStartDate { get; set; }
        public DateTime? ECTMP_TermEndDate { get; set; }
        public List<Exm_CCE_TERMS_MP_EXAMSDMO> attstudattstd { get; set; }

    }
}




