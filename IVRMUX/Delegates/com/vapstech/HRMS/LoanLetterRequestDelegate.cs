using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class LoanLetterRequestDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Emp_Loan_TransactionDTO, HR_Emp_Loan_TransactionDTO> COMMM = new CommonDelegate<HR_Emp_Loan_TransactionDTO, HR_Emp_Loan_TransactionDTO>();

        public HR_Emp_Loan_TransactionDTO onloadgetdetails(HR_Emp_Loan_TransactionDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "LoannonDeductLetterFacade/getalldetails");
        }

        public HR_Emp_Loan_TransactionDTO savedetails(HR_Emp_Loan_TransactionDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "LoannonDeductLetterFacade/");
        }
        public HR_Emp_Loan_TransactionDTO get_loans(HR_Emp_Loan_TransactionDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "LoannonDeductLetterFacade/get_loans");
        }
    }
}
