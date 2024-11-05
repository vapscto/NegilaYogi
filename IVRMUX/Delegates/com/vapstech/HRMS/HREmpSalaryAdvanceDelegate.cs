using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HREmpSalaryAdvanceDelegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate< HR_Emp_SalaryAdvanceDTO,  HR_Emp_SalaryAdvanceDTO> COMMM = new CommonDelegate< HR_Emp_SalaryAdvanceDTO,  HR_Emp_SalaryAdvanceDTO>();

    public  HR_Emp_SalaryAdvanceDTO onloadgetdetails( HR_Emp_SalaryAdvanceDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HREmpSalaryAdvanceFacade/onloadgetdetails");
    }

    public  HR_Emp_SalaryAdvanceDTO savedetails( HR_Emp_SalaryAdvanceDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpSalaryAdvanceFacade/");
    }
    public  HR_Emp_SalaryAdvanceDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HREmpSalaryAdvanceFacade/getRecordById/");
    }
    public  HR_Emp_SalaryAdvanceDTO deleterec( HR_Emp_SalaryAdvanceDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpSalaryAdvanceFacade/deactivateRecordById/");
    }

        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee(HR_Emp_SalaryAdvanceDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "HREmpSalaryAdvanceFacade/getDetailsByEmployee/");
            }


        public HR_Emp_SalaryAdvanceDTO searchfilter(HR_Emp_SalaryAdvanceDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmpSalaryAdvanceFacade/searchfilter/");
        }
    }
}
