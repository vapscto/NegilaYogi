using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Emp_Loan")]
  public class HR_Emp_Loan:CommonParamDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HREL_Id { get; set; }
    public long MI_Id { get; set; }

    public long HRME_Id { get; set; }
    public DateTime HREL_AppliedDate { get; set; }
    public string HREL_EmpLoanId { get; set; }
    public long HRMLN_Id { get; set; }
    public decimal? HREL_LoanAmount { get; set; }
    public string HREL_LoanInsallments { get; set; }
    public string HREL_LaonEMI { get; set; }
    public string HREL_LoanStatus { get; set; }
    public decimal? HREL_SanctionedAmount { get; set; }
    public decimal? HREL_TotalPrincipalPaid { get; set; }
    public decimal? HREL_TotalInterestPaid { get; set; }
    public decimal? HREL_TotalPending { get; set; }
    public decimal? HREL_LoanInterest { get; set; }
    public string HREL_ModeOfPayment { get; set; }
    public long? HREL_ReferenceNo { get; set; }
    public bool HREL_ActiveFlag { get; set; }
    [ForeignKey("HRMLN_Id")]
    public virtual HRMasterLoan HRMasterLoan { get; set; }
    [ForeignKey("HRME_Id")]
    public virtual MasterEmployee MasterEmployee { get; set; }

        public long HREL_NoofInstallment { get; set; }
    }
}
