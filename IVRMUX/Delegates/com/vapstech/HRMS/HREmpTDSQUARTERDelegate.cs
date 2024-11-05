using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HREmpTDSQUARTERDelegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<HR_Emp_TDS_QUARTERDTO, HR_Emp_TDS_QUARTERDTO> COMMM = new CommonDelegate<HR_Emp_TDS_QUARTERDTO, HR_Emp_TDS_QUARTERDTO>();

    public HR_Emp_TDS_QUARTERDTO onloadgetdetails(HR_Emp_TDS_QUARTERDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "HREmpTDSQUARTERFacade/onloadgetdetails");
    }

    public HR_Emp_TDS_QUARTERDTO savedetails(HR_Emp_TDS_QUARTERDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpTDSQUARTERFacade/");
    }
    public HR_Emp_TDS_QUARTERDTO getRecorddetailsById(int id)
    {
      return COMMM.GetDataByIdHRMS(id, "HREmpTDSQUARTERFacade/getRecordById/");
    }
    public HR_Emp_TDS_QUARTERDTO deleterec(HR_Emp_TDS_QUARTERDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "HREmpTDSQUARTERFacade/deactivateRecordById/");
    }
    public HR_Emp_TDS_QUARTERDTO getDetailsByEmployee(HR_Emp_TDS_QUARTERDTO maspage)
    {
    return COMMM.POSTDataHRMS(maspage, "HREmpTDSQUARTERFacade/getDetailsByEmployee/");
    }
        public HR_Emp_TDS_QUARTERDTO getquarter(HR_Emp_TDS_QUARTERDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HREmpTDSQUARTERFacade/getquarter/");
        }

    }
}
