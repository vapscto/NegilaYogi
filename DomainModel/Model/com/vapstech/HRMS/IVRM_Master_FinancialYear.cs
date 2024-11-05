using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("IVRM_Master_FinancialYear")]
    public class IVRM_Master_FinancialYear:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMFY_Id { get; set; }
        public DateTime? IMFY_FromDate { get; set; }
        public DateTime? IMFY_ToDate { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public string IMFY_AssessmentYear { get; set; }
        public long IMFY_OrderBy { get; set; }
    }
}
