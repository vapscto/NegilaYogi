using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HREmpTDSDelegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HR_Emp_TDSDTO, HR_Emp_TDSDTO> COMMM = new CommonDelegate<HR_Emp_TDSDTO, HR_Emp_TDSDTO>();

    public HR_Emp_TDSDTO onloadgetdetails(HR_Emp_TDSDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HREmpTDSFacade/onloadgetdetails");
    }

    public HR_Emp_TDSDTO savedetails(HR_Emp_TDSDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpTDSFacade/");
    }
    public HR_Emp_TDSDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HREmpTDSFacade/getRecordById/");
    }
    public HR_Emp_TDSDTO deleterec(HR_Emp_TDSDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpTDSFacade/deactivateRecordById/");
    }
        public HR_Emp_TDSDTO getDetailsByEmployee(HR_Emp_TDSDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "HREmpTDSFacade/getDetailsByEmployee/");
            }

        }
}
