using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_Master_EventsDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VBSC_Master_EventsDTO, VBSC_Master_EventsDTO> COMMC = new CommonDelegate<VBSC_Master_EventsDTO, VBSC_Master_EventsDTO>();
        public VBSC_Master_EventsDTO getloaddata(VBSC_Master_EventsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_EventsFacade/getloaddata/");
        }
        public VBSC_Master_EventsDTO savedetails(VBSC_Master_EventsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_EventsFacade/savedetails/");
        }
        public VBSC_Master_EventsDTO deactive(VBSC_Master_EventsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_EventsFacade/deactive/");
        }

    }
}
