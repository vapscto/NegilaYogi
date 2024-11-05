using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class FORMNO15GDelegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<FORMNO15GDTO, FORMNO15GDTO> COMMM = new CommonDelegate<FORMNO15GDTO, FORMNO15GDTO>();

    public FORMNO15GDTO onloadgetdetails(FORMNO15GDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "FORMNO15GFacade/onloadgetdetails");
    }


    //getEmployeedetailsBySelection  

    public FORMNO15GDTO getEmployeedetailsBySelection(FORMNO15GDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORMNO15GFacade/getEmployeedetailsBySelection/");
    }

    public FORMNO15GDTO FilterEmployeeData(FORMNO15GDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORMNO15GFacade/FilterEmployeeData/");
    }
  }
}
