using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class CTCReportDelegate
    {
    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<CTCReportDTO, CTCReportDTO> COMMM = new CommonDelegate<CTCReportDTO, CTCReportDTO>();

    public CTCReportDTO onloadgetdetails(CTCReportDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "CTCReportFacade/onloadgetdetails");
    }


    //getEmployeedetailsBySelection  

    public CTCReportDTO getEmployeedetailsBySelection(CTCReportDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "CTCReportFacade/getEmployeedetailsBySelection/");
    }

    public CTCReportDTO FilterEmployeeData(CTCReportDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "CTCReportFacade/FilterEmployeeData/");
    }
  }
}
