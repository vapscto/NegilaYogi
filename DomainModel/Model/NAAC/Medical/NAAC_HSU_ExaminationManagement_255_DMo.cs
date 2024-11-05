using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_HSU_ExaminationManagement_255")]
    public class NAAC_HSU_ExaminationManagement_255_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSUEM255_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSUEM255_Year { get; set; }
        public bool NCHSUEM255_AnDivImpEMFlag { get; set; }
        public bool NCHSUEM255_StuRegHtIssueProcessingFlag { get; set; }
        public bool NCHSUEM255_StuRegResultProcFlag { get; set; }
        public bool NCHSUEM255_ResultProcAtdFlag { get; set; }
        public bool NCHSUEM255_ManualMethodologyFlag { get; set; }
        public long NCHSUEM255_CreatedBy { get; set; }
        public long NCHSUEM255_UpdatedBy { get; set; }
        public DateTime NCHSUEM255_CreatedDate { get; set; }
        public DateTime NCHSUEM255_UpdatedDate { get; set; }
    }
}
