using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HREmpLoanDelegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HR_Emp_LoanDTO, HR_Emp_LoanDTO> COMMM = new CommonDelegate<HR_Emp_LoanDTO, HR_Emp_LoanDTO>();

    public HR_Emp_LoanDTO onloadgetdetails(HR_Emp_LoanDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HREmpLoanFacade/onloadgetdetails");
    }

    public HR_Emp_LoanDTO savedetails(HR_Emp_LoanDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpLoanFacade/");
    }
    public HR_Emp_LoanDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HREmpLoanFacade/getRecordById/");
    }
    public HR_Emp_LoanDTO deleterec(HR_Emp_LoanDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpLoanFacade/deactivateRecordById/");
    }
        public HR_Emp_LoanDTO getDetailsByEmployee(HR_Emp_LoanDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "HREmpLoanFacade/getDetailsByEmployee/");
            }

        }
}
