using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class LoanApprovalControllerDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Emp_Loan_ApprovalDTO, HR_Emp_Loan_ApprovalDTO> COMMM = new CommonDelegate<HR_Emp_Loan_ApprovalDTO, HR_Emp_Loan_ApprovalDTO>();

        public HR_Emp_Loan_ApprovalDTO onloadgetdetails(HR_Emp_Loan_ApprovalDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "LoanApprovalFacade/getalldetails");
        }

    }
}
