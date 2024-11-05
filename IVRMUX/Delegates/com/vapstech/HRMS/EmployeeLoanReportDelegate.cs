using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeLoanReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        //CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO> COMMM = new CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO>();
        CommonDelegate<LoanReportDTO, LoanReportDTO> COMMM1 = new CommonDelegate<LoanReportDTO, LoanReportDTO>();

        public LoanReportDTO onloadgetdetails(LoanReportDTO dto)
        {
            return COMMM1.POSTDataaHRMS(dto, "SalaryLoanReportFacade/onloadgetdetails");
        }

        //getEmployeedetailsBySelection  

        public LoanReportDTO getEmployeedetailsBySelection(LoanReportDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "SalaryLoanReportFacade/getEmployeedetailsBySelection/");
        }

        public LoanReportDTO get_depts(LoanReportDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "SalaryLoanReportFacade/get_depts/");
        }
        public LoanReportDTO get_desig(LoanReportDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "SalaryLoanReportFacade/get_desig/");
        }
    }
}

