using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Emp_Loan_Transaction")]
    public class HR_Emp_Loan_TransactionDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRELT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HREL_Id { get; set; }
        public int HRELT_Year { get; set; }
        public string HRELT_Month { get; set; }
        public bool HRELT_PaidFlag { get; set; }
        public decimal HRELT_LoanAmount { get; set; }
        public decimal HRELT_PrincipalAmount { get; set; }
        public decimal HRELT_InterestAmount { get; set; }
        public string HRELT_Reason { get; set; }

    }
}
