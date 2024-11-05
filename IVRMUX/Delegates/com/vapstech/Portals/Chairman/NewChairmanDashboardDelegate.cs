using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class NewChairmanDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NewChairmanDashboardDTO, NewChairmanDashboardDTO> COMMM = new CommonDelegate<NewChairmanDashboardDTO, NewChairmanDashboardDTO>();
        public NewChairmanDashboardDTO Getdetails(NewChairmanDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NewChairmanDashboardFacade/Getdetails/");
        }

        public NewChairmanDashboardDTO ViewFiles(NewChairmanDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NewChairmanDashboardFacade/ViewFiles");
        }

    }
}
