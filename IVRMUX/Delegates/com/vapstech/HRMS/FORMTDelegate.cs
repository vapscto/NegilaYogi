using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class FORMTDelegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<FORMTDTO, FORMTDTO> COMMM = new CommonDelegate<FORMTDTO, FORMTDTO>();

    public FORMTDTO onloadgetdetails(FORMTDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "FORMTFacade/onloadgetdetails");
    }


    //getEmployeedetailsBySelection  

    public FORMTDTO getEmployeedetailsBySelection(FORMTDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORMTFacade/getEmployeedetailsBySelection/");
    }

    public FORMTDTO FilterEmployeeData(FORMTDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORMTFacade/FilterEmployeeData/");
    }
  }
}
