using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_EvaluationRelated_253")]
    public class NAAC_HSU_EvaluationRelated_253_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU253ER_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSU253ER_Year { get; set; }
        public long NCHSU253ER_TotalNoOfStsAppreadFinalExm { get; set; }
        public long NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation { get; set; }
        public long NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly { get; set; }
        public long NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly { get; set; }
        public long NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript { get; set; }
        public bool NCHSU253ER_ActiveFlag { get; set; }
        public DateTime NCHSU253ER_CreatedDate { get; set; }
        public DateTime NCHSU253ER_UpdatedDate { get; set; }
        public long NCHSU253ER_CreatedBy { get; set; }
        public long NCHSU253ER_UpdatedBy { get; set; }
        public List<NAAC_HSU_EvaluationRelated_253_Files_DMO> NAAC_HSU_EvaluationRelated_253_Files_DMO { get; set; }
    }
}
