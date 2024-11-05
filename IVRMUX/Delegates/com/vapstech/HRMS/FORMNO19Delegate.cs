using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class FORMNO19Delegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<FORMNO19DTO, FORMNO19DTO> COMMM = new CommonDelegate<FORMNO19DTO, FORMNO19DTO>();

    public FORMNO19DTO onloadgetdetails(FORMNO19DTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "FORMNO19Facade/onloadgetdetails");
    }


    //getEmployeedetailsBySelection  

    public FORMNO19DTO getEmployeedetailsBySelection(FORMNO19DTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORMNO19Facade/getEmployeedetailsBySelection/");
    }

    public FORMNO19DTO FilterEmployeeData(FORMNO19DTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "FORMNO19Facade/FilterEmployeeData/");
    }
  }
}
