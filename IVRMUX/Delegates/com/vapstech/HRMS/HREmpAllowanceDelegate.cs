using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HREmpAllowanceDelegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HR_Emp_AllowanceDTO, HR_Emp_AllowanceDTO> COMMM = new CommonDelegate<HR_Emp_AllowanceDTO, HR_Emp_AllowanceDTO>();

    public HR_Emp_AllowanceDTO onloadgetdetails(HR_Emp_AllowanceDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HREmpAllowanceFacade/onloadgetdetails");
    }

    public HR_Emp_AllowanceDTO savedetails(HR_Emp_AllowanceDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpAllowanceFacade/");
    }
    public HR_Emp_AllowanceDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HREmpAllowanceFacade/getRecordById/");
    }
    public HR_Emp_AllowanceDTO deleterec(HR_Emp_AllowanceDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpAllowanceFacade/deactivateRecordById/");
    }
        public HR_Emp_AllowanceDTO getDetailsByEmployee(HR_Emp_AllowanceDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "HREmpAllowanceFacade/getDetailsByEmployee/");
            }

        }
}
