using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_T_Installment_DueDate")]
    public class Clg_Fee_Installment_Due_Date_DMO : CommonParamDMO
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
