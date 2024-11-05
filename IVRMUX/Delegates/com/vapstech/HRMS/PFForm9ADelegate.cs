using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class PFForm9ADelegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<PFReportsDTO, PFReportsDTO> COMMM = new CommonDelegate<PFReportsDTO, PFReportsDTO>();

    public PFReportsDTO onloadgetdetails(PFReportsDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "PFForm9AFacade/onloadgetdetails");
    }

    //getEmployeedetailsBySelection  

    public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "PFForm9AFacade/getEmployeedetailsBySelection/");
    }

    public PFReportsDTO FilterEmployeeData(PFReportsDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "PFForm9AFacade/FilterEmployeeData/");
    }

  }
}
