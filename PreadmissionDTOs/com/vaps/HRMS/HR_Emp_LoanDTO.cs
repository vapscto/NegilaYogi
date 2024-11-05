using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_LoanDTO : CommonParamDTO
    {
    public long HREL_Id { get; set; }
    public long MI_Id { get; set; }

    public long HRME_Id { get; set; }
    public DateTime? HREL_AppliedDate { get; set; }
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
    public Array emploanList { get; set; }
    public string retrunMsg { get; set; }
    public long roleId { get; set; }

    public Array employeedropdown { get; set; }

    public Array masterloandropdown { get; set; }

 
    public string hrmE_EmployeeFirstName { get; set; }

        public string HRML_LoanType { get; set; }

  

    public Array modeOfPaymentdropdown { get; set; }
    public HR_ConfigurationDTO configurationDetails { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }

        //Academic Year
             public long ASMAY_Id { get; set; }
        public decimal? empGrossSal { get; set; }

        public long HREL_NoofInstallment { get; set; }

        }

}
