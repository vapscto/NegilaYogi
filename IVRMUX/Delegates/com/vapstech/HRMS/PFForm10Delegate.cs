using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class PFForm10Delegate
    {

    private readonly object resource;
    private readonly string serviceBaseUrl;
    private const String JsonContentType = "application/json; charset=utf-8";
    CommonDelegate<PFReportsDTO, PFReportsDTO> COMMM = new CommonDelegate<PFReportsDTO, PFReportsDTO>();

    public PFReportsDTO onloadgetdetails(PFReportsDTO dto)
    {
      return COMMM.POSTDataHRMS(dto, "PFForm10Facade/onloadgetdetails");
    }

    //getEmployeedetailsBySelection  

    public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "PFForm10Facade/getEmployeedetailsBySelection/");
    }

    public PFReportsDTO FilterEmployeeData(PFReportsDTO maspage)
    {
      return COMMM.POSTDataHRMS(maspage, "PFForm10Facade/FilterEmployeeData/");
    }

        public PFReportsDTO getEmployeedetailsBySelectionStjames(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PFForm10Facade/getEmployeedetailsBySelectionStjames/");
        }

        
    }
}
