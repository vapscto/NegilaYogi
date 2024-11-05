using DomainModel.Model.com.vapstech.Exam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{

    [Table("Exm_CCE_TERMS", Schema = "Exm")]
    public class CCE_Exam_M_TermsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ECT_Id { get; set; }       
        public long MI_Id { get; set; }      
        public string ECT_TermName { get; set; }     
        public bool ECT_ActiveFlag { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal ECT_Marks { get; set; }
        public DateTime? ECT_TermStartDate { get; set; }
        public DateTime? ECT_TermEndDate { get; set; }
        public decimal? ECT_MinMarks { get; set; }
        public DateTime? ECT_PublishDate { get; set; }
        public List<Exm_CCE_TERMS_EXAMSDMO> Exm_CCE_TERMS_EXAMSDMO { get; set; }
    }
}




