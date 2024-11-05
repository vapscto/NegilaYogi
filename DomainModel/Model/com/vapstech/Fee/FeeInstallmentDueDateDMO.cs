using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_T_Installment_DueDate")]
    public class FeeInstallmentDueDateDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FTIDD_Id { get; set; }
        public long MI_Id { get; set; }
        public long FTI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FTIDD_FromDate { get; set; }
        public DateTime FTIDD_ToDate { get; set; }
        public DateTime FTIDD_ApplicableDate { get; set; }
        public DateTime FTIDD_DueDate { get; set; }
    }
}
