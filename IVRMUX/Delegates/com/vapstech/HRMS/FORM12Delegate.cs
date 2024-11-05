using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class FORM12Delegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<FORM12DTO, FORM12DTO> COMMM = new CommonDelegate<FORM12DTO, FORM12DTO>();

    public FORM12DTO onloadgetdetails(FORM12DTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "FORM12Facade/onloadgetdetails");
    }


    //getEmployeedetailsBySelection  

    public FORM12DTO getEmployeedetailsBySelection(FORM12DTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORM12Facade/getEmployeedetailsBySelection/");
    }

    public FORM12DTO FilterEmployeeData(FORM12DTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORM12Facade/FilterEmployeeData/");
    }

  }
}
