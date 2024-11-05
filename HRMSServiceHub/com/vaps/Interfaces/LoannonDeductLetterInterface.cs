using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface LoannonDeductLetterInterface
    {

        HR_Emp_Loan_TransactionDTO getBasicData(HR_Emp_Loan_TransactionDTO dto);

        HR_Emp_Loan_TransactionDTO SaveUpdate(HR_Emp_Loan_TransactionDTO dto);
        HR_Emp_Loan_TransactionDTO get_loans(HR_Emp_Loan_TransactionDTO dto);
    }
}
